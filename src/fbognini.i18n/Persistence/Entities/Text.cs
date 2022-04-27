using System;
using System.Collections.Generic;

namespace fbognini.i18n.Persistence.Entities
{
    public class Text
    {
        public string TextId { get; set; }
        public string ResourceId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        public ICollection<Translation> Translations { get; set; }
    }
}
