using fbognini.i18n.Dashboard.Handlers.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fbognini.i18n.Dashboard.i18n.Pages
{
    public class TranslationModel : PageModel
    {
        public List<TranslationDto> Translations { get; set; } = new();
    }
}