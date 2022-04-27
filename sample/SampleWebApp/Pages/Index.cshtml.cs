using fbognini.i18n;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly II18nRepository i18NRepository;

        public IndexModel(ILogger<IndexModel> logger, II18nRepository i18NRepository)
        {
            _logger = logger;
            this.i18NRepository = i18NRepository;
        }

        public async Task OnGet()
        {
            //await i18NRepository.AddText("chiave2", "TEXT", "descrizione", new Dictionary<string, string>()
            //{
            //    ["it-IT"] = "italiano2",
            //    ["en-GB"] = "inglese2",
            //});

            var language = Thread.CurrentThread.CurrentUICulture.Name;

            var translations = await i18NRepository.GetTranslations(language);
        }
    }
}