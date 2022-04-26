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

        public async Task<List<Language>> GetLanguages(bool isActive, CancellationToken cancellationToken = default)
        {
            var query = context.Languages.AsQueryable();
            if (isActive)
            {
                query = query.Where(x => x.IsActive);
            }

            return await query.ToListAsync(cancellationToken);
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

        public async Task AddText(string id, string group, string description, Dictionary<string, string> translations, CancellationToken cancellationToken = default)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var now = DateTime.Now;

                var text = new Text()
                {
                    Id = id,
                    Group = group,
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
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }

        public async Task<Dictionary<string, string>> GetTranslations(string language, string group = null, DateTime? since = null, CancellationToken cancellationToken = default)
        {
            var query = context.Translations
                .AsNoTracking()
                .Include(x => x.Text)
                .Where(x => x.LanguageId == language);

            if (!string.IsNullOrWhiteSpace(group))
            {
                query = query.Where(x => x.Text.Group == group);
            }

            if (since.HasValue)
            {
                query = query.Where(x => x.Updated >= since.Value);
            }

            var translations = await query.ToListAsync(cancellationToken);

            return translations.ToDictionary(x => x.Text.Id, x => x.Destination);
        }
    }
}
