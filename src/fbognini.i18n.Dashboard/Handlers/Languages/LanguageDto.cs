using fbognini.AutoMapper.Mappings;
using fbognini.i18n.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Handlers.Languages
{
    public class LanguageDto : Mappable<LanguageDto, Language>
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
