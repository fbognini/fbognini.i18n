using fbognini.i18n.Dashboard.Handlers.Texts;
using fbognini.WebFramework.FullSearch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Areas.i18n.Controllers
{
    [Area("i18n")]
    public class ApiTextController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult> Search([FromQuery] GetPaginatedTextsQuery query, [FromQuery] FullSearchQueryParameters search)
        {
            query.LoadFullSearchParameters(search);

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateTextCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] DeleteTextCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}