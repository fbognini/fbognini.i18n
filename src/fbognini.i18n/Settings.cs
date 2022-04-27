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

    internal class Settings
    {
        public Settings()
        {
            Context = new ContextSettings();
        }
        public string ConnectionString { get; set; }
        public bool UseCache { get; set; }
        public bool UseRequestLocalization { get; set; }
        public ContextSettings Context { get; set; }

    }
}
