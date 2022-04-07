using fbognini.i18n.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace fbognini.i18n
{
    public class i18nRepository : Ii18nRepository
    {
        private i18nContext context;
        private string baseUriResource;

        public i18nRepository(i18nContext context)
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
            //.Where(x => x.IsActive)
            .Select(x => x.Id).ToList();
    }
}
