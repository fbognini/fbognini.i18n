using System.Collections.Generic;

namespace fbognini.i18n
{

    

    public class I18nSettings
    {
        public class LocalizerSettings
        {
            public string OverrideResourceId { get; set; }
            public string BaseResourceId { get; set; }
            public List<string> RemovePrefixs { get; set; } = new List<string>();
            public List<string> RemoveSuffixs { get; set; } = new List<string>();

            public bool CreateNewRecordWhenDoesNotExists { get; set; }
        }

        public string ConnectionString { get; set; }
        public string Schema { get; set; } = "i18n";
        public bool UseCache { get; set; } = true;
        public string CookieName { get; set; }
        public LocalizerSettings Localizer { get; set; } = new LocalizerSettings();
    }
}
