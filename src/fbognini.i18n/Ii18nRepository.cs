using fbognini.i18n.Persistence.Entities;
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

        Task<List<Language>> GetLanguages(bool isActive, CancellationToken cancellationToken = default);

        Task<int> NewTranslation(int? id = null, string defaultString = null, CancellationToken cancellationToken = default);
    }
}
