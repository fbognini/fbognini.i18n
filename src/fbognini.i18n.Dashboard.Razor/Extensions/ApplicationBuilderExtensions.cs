using fbognini.i18n.Dashboard.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseI18nDashboard(this WebApplication app, bool authorizeApi)
        {
            var endpointBulder = app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            if (authorizeApi)
            {
                endpointBulder.RequireAuthorization(I18nDashboardPolicy.Dashboard);
            }

            return app;
        }
    }
}
