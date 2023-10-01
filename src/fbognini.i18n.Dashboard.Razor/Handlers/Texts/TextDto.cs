using fbognini.AutoMapper.Mappings;
using fbognini.i18n.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    public class TextDto : Mappable<TextDto, Text>
    {
        public string TextId { get; set; }
        public string ResourceId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
