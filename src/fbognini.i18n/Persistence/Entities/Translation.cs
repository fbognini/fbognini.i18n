namespace fbognini.i18n.Persistence.Entities
{
    public class Translation
    {
        public string LanguageId { get; set; }
        public int Source { get; set; }
        public string Destination { get; set; }
    }
}
