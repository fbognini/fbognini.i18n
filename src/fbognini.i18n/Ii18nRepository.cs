using fbognini.i18n.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace fbognini.i18n
{
    public interface II18nRepository
    {
        string BaseUriResource { get; }
        string Translate(string language, int source);
        List<string> Languages { get; }


        internal void DetachAllEntities();

        IEnumerable<Translation> AddTranslations(string textId, string resourceId, string description, Dictionary<string, string> translations);
        IEnumerable<Translation> GetTranslations(string languageId, string textId, string resourceId, DateTime? since = null);
        IEnumerable<Language> GetLanguages();
    }
}
