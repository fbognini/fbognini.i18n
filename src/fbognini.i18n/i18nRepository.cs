using fbognini.i18n.Persistence;
using fbognini.i18n.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Snickler.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

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

        //public string Translate(string language, int source)
        //{
        //    var entity = context.Translations.Find(language, source);
        //    return entity?.Destination;
        //}

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
    }
}
