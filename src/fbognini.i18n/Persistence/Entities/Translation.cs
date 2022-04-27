using System;

namespace fbognini.i18n.Persistence.Entities
{
    public class Translation
    {
        public string LanguageId { get; set; }
        public string TextId { get; set; }
        public string ResourceId { get; set; }
        public string Destination { get; set; }
        public DateTime Updated { get; set; }

        public Language Language { get; set; }
        public Text Text { get; set; }
    }
}
