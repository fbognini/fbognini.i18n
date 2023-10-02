using fbognini.Core.Data;
using fbognini.Core.Data.Pagination;
using fbognini.i18n.Persistence;
using fbognini.i18n.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Snickler.EFCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Text = fbognini.i18n.Persistence.Entities.Text;

namespace fbognini.i18n
{
    internal class I18nRepository : II18nRepository
    {
        private List<Language> languages;

        private I18nContext context;
        private string baseUriResource;

        public I18nRepository(I18nContext context)
        {
            this.context = context;
        }

        public string BaseUriResource
        {
            get
            {
                if (string.IsNullOrWhiteSpace(baseUriResource))
                    baseUriResource = context.Configurations.FirstOrDefault()?.BaseUriResource;

                return baseUriResource;
            }
        }

        public List<string> Languages => context.Languages
            .Where(x => x.IsActive)
            .Select(x => x.Id).ToList();

        public IEnumerable<Language> GetLanguages()
        {
            if (languages == null)
            {
                lock (context)
                {
                    languages = context.Languages.ToList();
                }
            }

            return languages;
        }

        public PaginationResponse<Language> GetPaginatedLanguages(SelectCriteria<Language> criteria) => GetPaginatedResponse<Language>(criteria);

        public void AddLanguage(Language language)
        {
            lock (context)
            {
                _AddLanguage(language);
            }
        }

        public Language UpdateLanguage(string id, string description, bool isActive, bool isDefault)
        {
            var language = context.Languages.Find(id);
            if (language is null)
            {
                throw new ArgumentException($"Invalid language {id}");
            }

            language.Description = description;
            language.IsActive = isActive;
            language.IsDefault = isDefault;

            lock (context)
            {
                context.Languages.Update(language);
                context.SaveChanges();
            }

            return language;
        }

        public void AddLanguageWithTranslations(Language language)
        {
            lock (context)
            {
                using var transaction = context.Database.BeginTransaction();

                try
                {
                    _AddLanguage(language);

                    context.Database.ExecuteSqlRaw("EXEC [i18n].[AddTranslationsForLanguage] @LanguageId = {0}", language.Id);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<Translation> GetTranslations(string languageId, string textId, string resourceId, DateTime? since = null)
        {
            (this as II18nRepository).DetachAllEntities();

            lock (context)
            {
                var query = context.Translations.AsQueryable();
                if (!string.IsNullOrWhiteSpace(languageId))
                {
                    query = query.Where(x => x.LanguageId == languageId);
                }
                if (!string.IsNullOrWhiteSpace(textId))
                {
                    query = query.Where(x => x.TextId == textId);
                }
                if (!string.IsNullOrWhiteSpace(resourceId))
                {
                    query = query.Where(x => x.ResourceId == resourceId);
                }
                if (since.HasValue)
                {
                    query = query.Where(x => x.Updated >= since.Value);
                }

                return query.ToList();
            }
        }

        public PaginationResponse<Translation> GetPaginatedTranslations(SelectCriteria<Translation> criteria) => GetPaginatedResponse<Translation>(criteria);

        public PaginationResponse<Text> GetPaginatedTexts(SelectCriteria<Text> criteria) => GetPaginatedResponse<Text>(criteria);

        public IEnumerable<Translation> AddTranslations(string textId, string resourceId, string description, Dictionary<string, string> translations)
        {
            if (translations == null || !translations.Any())
                throw new ArgumentException("Translations must be provided");

            var languages = GetLanguages();
            var invalid = translations.Where(t => !languages.Any(l => l.Id == t.Key)).ToList();
            if (invalid.Any())
                throw new ArgumentException($"Invalid languages [{string.Join(", ", invalid.Select(x => x.Key))}]");

            var defaultLanguage = languages.FirstOrDefault(x => x.IsDefault);
            if (defaultLanguage == null)
            {
                defaultLanguage = languages.First();
            }

            var defaultTranslation = translations.ContainsKey(defaultLanguage.Id) ? translations[defaultLanguage.Id] : translations.First().Value;
            
            foreach (var item in languages.Where(x => !translations.ContainsKey(x.Id)))
            {
                translations.Add(item.Id, defaultTranslation);
            }
            
            var now = DateTime.Now;

            var text = new Text()
            {
                TextId = textId,
                ResourceId = resourceId,
                Description = description,
                Created = now,
                Translations = translations.Select(x => new Translation()
                {
                    TextId = textId,
                    ResourceId = resourceId,
                    LanguageId = x.Key,
                    Destination = x.Value ?? string.Empty,
                    Updated = now
                }).ToList()
            };

            lock (context)
            {
                context.Texts.Add(text);
                context.SaveChanges();
            }

            return text.Translations;
        }

        public void DeleteTranslations(string textId, string resourceId)
        {
            lock (context)
            {
                context.Translations.RemoveRange(context.Translations.Where(x => x.TextId == textId && x.ResourceId == resourceId));
                context.Texts.Remove(context.Texts.Find(textId, resourceId));
                context.SaveChanges();
            }
        }

        public void UpdateTranslation(Translation translation)
        {
            UpdateTranslation(translation, true);
        }

        public void UpdateTranslations(List<Translation> translations)
        {
            foreach (var translation in translations)
            {
                UpdateTranslation(translation, false);
            }

            lock (context)
            {
                context.SaveChanges();
            }
        }

        public void ImportExcel(string path, bool all, bool deletenotmatched)
        {
            var translations = GetExcelRecords(path);

            ImportTranslations(translations, all, deletenotmatched);
        }

        public byte[] ExportExcel()
        {
            var translations = GetTranslations(null, null, null, null);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Texts");

            var header = new List<string[]>
                    {
                        new string[] {
                            "LanguageId",
                            "TextId",
                            "ResourceId",
                            "Destination",
                            "Updated", },
                    };

            var headerRange = worksheet.Cells["A1:E1"];
            headerRange.Style.Font.Bold = true;
            headerRange.LoadFromArrays(header);

            worksheet.Cells[2, 1, translations.Count() + 1, 8]
                .LoadFromArrays(translations.Select(x => new string[] { x.LanguageId, x.TextId, x.ResourceId, x.Destination, x.Updated.ToString() }).ToArray());

            return package.GetAsByteArray();
        }

        public void ImportTranslations(IEnumerable<Translation> translations, bool all, bool deletenotmatched)
        {
            var now = DateTime.Now;

            lock (context)
            {
                var existing = GetTranslations(null, null, null, null);
                foreach (var existingTranslation in existing)
                {
                    var newTranslation = translations
                        .FirstOrDefault(x => x.LanguageId == existingTranslation.LanguageId && x.ResourceId == existingTranslation.ResourceId && x.TextId == existingTranslation.TextId);

                    if (newTranslation == null)
                    {
                        if (deletenotmatched)
                        {
                            context.Translations.Remove(existingTranslation);
                        }

                        continue;
                    }

                    if (!all && existingTranslation.Updated > newTranslation.Updated)
                    {
                        continue;
                    }

                    if (!existingTranslation.Destination.Equals(newTranslation.Destination))
                    {
                        existingTranslation.Updated = now;
                        existingTranslation.Destination = newTranslation.Destination;
                    }
                }

                context.SaveChanges();
            }
        }

        void II18nRepository.DetachAllEntities()
        {
            lock (context)
            {
                context.DetachAllEntities();
            }
        }

        private void _AddLanguage(Language language)
        {
            context.Languages.Add(language);

            context.SaveChanges();
        }

        private void UpdateTranslation(Translation translation, bool saveChanges = true)
        {
            var entity = context.Translations.Find(translation.LanguageId, translation.TextId, translation.ResourceId);
            if (entity == null)
                return;

            entity.Destination = translation.Destination;
            entity.Updated = DateTime.Now;
            context.Translations.Update(entity);

            if (!saveChanges)
                return;

            lock (context)
            {
                context.SaveChanges();
            }
        }

        private static List<Translation> GetExcelRecords(string path)
        {
            using var package = new ExcelPackage(new FileInfo(path));
            var worksheet = package.Workbook.Worksheets[0];

            int rowCount = worksheet.Dimension.End.Row;

            var records = new List<Translation>();
            for (int i = 2; i <= rowCount; i++)
            {
                var languageId = worksheet.Cells[i, 1].Value?.ToString().Trim();
                var textId = worksheet.Cells[i, 2].Value?.ToString().Trim();
                var resourceId = worksheet.Cells[i, 3].Value?.ToString().Trim();
                var destination = worksheet.Cells[i, 4].Value?.ToString().Trim();
                var updated = worksheet.Cells[i, 5].Value?.ToString().Trim();

                records.Add(new Translation
                {
                    LanguageId = languageId,
                    TextId = textId,
                    ResourceId = resourceId,
                    Destination = destination,
                    Updated = DateTime.Parse(updated)
                });
            }

            return records;
        }

        private PaginationResponse<T> GetPaginatedResponse<T>(SelectCriteria<T> criteria)
            where T : class
        {
            lock (context)
            {
                var query = context.Set<T>().QuerySelect(criteria).QuerySearch(criteria, out var pagination);
                var list = query.ToList();
                var response = new PaginationResponse<T>()
                {
                    Pagination = pagination,
                    Items = list
                };

                return response;
            }
        }
    }
}
