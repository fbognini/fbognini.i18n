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

        Task<List<Language>> GetLanguages(CancellationToken cancellationToken = default);
        Task AddText(string id, string group, string description, Dictionary<string, string> translations, CancellationToken cancellationToken = default);
        Task<Dictionary<string, string>> GetTranslations(string language, string group = null, DateTime? since = null, CancellationToken cancellationToken = default);
    }
}
