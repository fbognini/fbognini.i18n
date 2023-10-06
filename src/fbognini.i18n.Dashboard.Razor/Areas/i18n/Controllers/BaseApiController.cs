using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Areas.i18n.Controllers
{
    [Area(DashboardContants.Area)]
    public abstract class BaseApiController : Controller
    {
        private IMediator? mediator;

        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
