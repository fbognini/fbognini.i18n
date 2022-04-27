using System.Collections.Generic;

namespace fbognini.i18n
{
    internal class ContextSettings
    {
        public ContextSettings()
        {
            Schema = "i18n";
        }

        public string Schema { get; set; }
    }

    internal class LocalizerSettings
    {
        public LocalizerSettings()
        {
            RemovePrefixs = new List<string>();
            RemoveSuffixs = new List<string>();
        }

        public string OverrideResourceId { get; set; }
        public string BaseResourceId { get; set; }
        public List<string> RemovePrefixs { get; set; }
        public List<string> RemoveSuffixs { get; set; }

        public bool CreateNewRecordWhenDoesNotExists { get; set; }
    }

    internal class Settings
    {
        public Settings()
        {
            Context = new ContextSettings();
        }
        public string ConnectionString { get; set; }
        public bool UseCache { get; set; }
        public ContextSettings Context { get; set; }
        public LocalizerSettings Localizer { get; set; }
    }
}
