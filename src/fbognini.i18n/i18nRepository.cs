using fbognini.i18n.Persistence;
using Snickler.EFCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fbognini.i18n
{
    public class I18nRepository : II18nRepository
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

        public async Task<int> NewTranslation(int? id = null, string defaultString = null, CancellationToken cancellationToken = default)
        {
            await context.LoadStoredProc($"[i18n].[NewTranslation]", commandTimeout: 120)
                .WithSqlParam("@Id", id)
                .WithSqlParam("@DefaultString", defaultString)
                .ExecuteStoredProcAsync(System.Data.CommandBehavior.Default, cancellationToken, true, (handler) =>
                {
                    id = handler.ReadToValue<int>();
                });

            return id.Value;
        }
    }
}
