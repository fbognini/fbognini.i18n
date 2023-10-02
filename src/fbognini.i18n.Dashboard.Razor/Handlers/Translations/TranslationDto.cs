using fbognini.AutoMapper.Mappings;
using fbognini.i18n.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Handlers.Translations
{
    public class TranslationDto : Mappable<TranslationDto, Translation>
    {
        public string Id => $"{LanguageId}|{TextId}|{ResourceId}";
        public required string LanguageId { get; set; }
        public required string TextId { get; set; }
        public required string ResourceId { get; set; }
        public required string Destination { get; set; }
        public DateTime Updated { get; set; }
    }
}
