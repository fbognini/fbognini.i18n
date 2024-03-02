using fbognini.i18n.Dashboard.Handlers.Languages;
using fbognini.i18n.Dashboard.Handlers.Texts;
using fbognini.i18n.Persistence.Entities;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Helpers
{
    internal static class TextMappings
    {
        public static TextDto ToDto(this Text text)
        {
            var dto = new TextDto()
            {
                TextId = text.TextId,
                ResourceId = text.ResourceId,
                Description = text.Description,
                Created = text.Created,
            };

            return dto;
        }
    }
}
