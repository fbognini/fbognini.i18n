using DocumentFormat.OpenXml.Wordprocessing;
using fbognini.i18n.Dashboard.Handlers.Languages;
using fbognini.i18n.Dashboard.Handlers.Texts;
using fbognini.i18n.Dashboard.Handlers.Translations;
using fbognini.i18n.Persistence.Entities;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Helpers
{
    internal static class TranslationMappings
    {
        public static TranslationDto ToDto(this Translation translation)
        {
            var dto = new TranslationDto()
            {
                LanguageId = translation.LanguageId,
                TextId = translation.TextId,
                ResourceId = translation.ResourceId,
                Destination = translation.Destination,
                Updated = translation.Updated,
            };

            return dto;
        }
    }
}
