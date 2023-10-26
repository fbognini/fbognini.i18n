using fbognini.i18n.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Controllers
{
    public class DispatcherController: Controller
    {
        public IActionResult Index(string view)
        {
            if (string.IsNullOrWhiteSpace(view))
            {
                return RedirectToAction(nameof(Index), "Languages", new { Area = DashboardContants.Area });
            }

            return View(view);
        }
    }
}
