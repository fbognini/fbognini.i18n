using System.Collections.Generic;

namespace fbognini.i18n
{
    public interface Ii18nRepository
    {
        string BaseUriResource { get; }
        string Translate(string language, int source);
        List<string> Languages { get; }
    }
}
