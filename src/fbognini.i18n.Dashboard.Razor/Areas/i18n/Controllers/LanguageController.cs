using fbognini.i18n.Dashboard.Handlers.Languages;
using fbognini.WebFramework.FullSearch;
using Microsoft.AspNetCore.Mvc;

namespace fbognini.i18n.Dashboard.Areas.i18n.Controllers
{
    [Area("i18n")]
    public class ApiLanguageController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult> Search([FromQuery] GetPaginatedLanguagesQuery query, [FromQuery] FullSearchQueryParameters search)
        {
            query.LoadFullSearchParameters(search);

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateLanguageCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateLanguageCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}