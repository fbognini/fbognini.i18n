using fbognini.i18n.Dashboard.Handlers.Translations;
using fbognini.WebFramework.FullSearch;
using Microsoft.AspNetCore.Mvc;

namespace fbognini.i18n.Dashboard.Areas.i18n.Controllers
{
    public class ApiTranslationController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult> Search([FromQuery] GetPaginatedTranslationsQuery query, [FromQuery] FullSearchQueryParameters search)
        {
            query.LoadFullSearchParameters(search);

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateTranslationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}