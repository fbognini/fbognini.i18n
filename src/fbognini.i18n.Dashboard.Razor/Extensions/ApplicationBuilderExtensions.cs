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
        public static IApplicationBuilder UseI18nDashboard(this WebApplication app)
        {
            //app.UseRouting();

            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //app.MapRazorPages();

            return app;
        }
    }
}
