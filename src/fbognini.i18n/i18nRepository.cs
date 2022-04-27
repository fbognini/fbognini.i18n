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

        public string Translate(string language, int source)
        {
            var entity = context.Translations.Find(language, source);
            return entity?.Destination;
        }

        public List<string> Languages => context.Languages
            .Where(x => x.IsActive)
            .Select(x => x.Id).ToList();

        public async Task<IEnumerable<Language>> GetLanguages(CancellationToken cancellationToken = default)
        {
            return await context.Languages.ToListAsync(cancellationToken);
        }

        //public async Task<int> GetNextSequence(string id = null, CancellationToken cancellationToken = default)
        //{
        //    if (id == null)
        //    {
        //        id = "TRA";
        //    }

        //    //using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.);

        //    //try
        //    //{
        //    //    var configuration = context.Configurations.Find(id);
        //    //    var source = configuration.Sequence;

        //    //    configuration.Sequence = configuration.Sequence + 1;
        //    //    await context.SaveChangesAsync(cancellationToken);
        //    //    // Commit transaction if all commands succeed, transaction will auto-rollback
        //    //    // when disposed if either commands fails
        //    //    transaction.Commit();

        //    //    return source;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    transaction.Rollback();

        //    //    throw;
        //    //}
        //}

        public async Task<IEnumerable<string>> AddText(string textId, string resourceId, string description, Dictionary<string, string> translations, CancellationToken cancellationToken = default)
        {
            if (translations == null || !translations.Any())
                throw new ArgumentException("Translations must be provided");

            var languages = await GetLanguages(cancellationToken);
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
            
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var now = DateTime.Now;

                var text = new Text()
                {
                    TextId = textId,
                    ResourceId = resourceId,
                    Description = description,
                    Translations = translations.Select(x => new Translation()
                    {
                        LanguageId = x.Key,
                        Destination = x.Value,
                        Updated = now
                    }).ToList(),
                    Created = now
                };

                await context.Texts.AddAsync(text, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return translations.Select(x => x.Key);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }

        public async Task<Dictionary<string, string>> GetTranslations(string languageId, string resourceId = null, DateTime? since = null, CancellationToken cancellationToken = default)
        {
            var language = await context.Languages.FindAsync(new[] { languageId }, cancellationToken: cancellationToken);
            if (language == null)
            {
                language = await context.Languages.FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);
                if (language == null)
                {
                    return new Dictionary<string, string>();
                }

                languageId = language.Id;
            }

            var query = context.Translations
                .AsNoTracking()
                .Include(x => x.Text)
                .Where(x => x.LanguageId == languageId);

            if (!string.IsNullOrWhiteSpace(resourceId))
            {
                query = query.Where(x => x.Text.ResourceId == resourceId);
            }

            if (since.HasValue)
            {
                query = query.Where(x => x.Updated >= since.Value);
            }

            var translations = await query.ToListAsync(cancellationToken);

            return translations.ToDictionary(x => x.Text.TextId, x => x.Destination);
        }
    }
}
