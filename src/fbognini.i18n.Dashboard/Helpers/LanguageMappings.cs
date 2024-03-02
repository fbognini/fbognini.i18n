using fbognini.i18n.Dashboard.Handlers.Languages;
using fbognini.i18n.Persistence.Entities;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Helpers
{
    internal static class LanguageMappings
    {
        public static LanguageDto ToDto(this Language language)
        {
            var dto = new LanguageDto()
            {
                Id = language.Id,
                Description = language.Description,
                IsActive = language.IsActive,
                IsDefault = language.IsDefault
            };

            return dto;
        }
    }
}
