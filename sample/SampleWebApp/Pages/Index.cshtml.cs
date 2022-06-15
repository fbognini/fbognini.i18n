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

            var language = Thread.CurrentThread.CurrentCulture.Name;
            DateTime? date = new DateTime(1970, 1, 1).AddTicks(1654512062397 * 10000);

            var ciccio = i18NRepository.GetTranslations("it-IT", null, "Site", date);

            i18NRepository.AddTranslations("chiave2", "TEXT", "descrizione", new Dictionary<string, string>()
            {
                ["it-IT"] = "italiano2",
                ["en-GB"] = "inglese2",
            });
        }
    }
}