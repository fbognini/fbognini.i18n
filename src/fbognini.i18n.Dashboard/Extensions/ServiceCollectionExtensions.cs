using fbognini.WebFramework.Handlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace fbognini.i18n.Dashboard.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddI18nDashboardServices(this IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddValidatorsFromAssemblyContaining<IMarker>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<IMarker>();
            });
            services.AddMediatRExceptionHandler();

            return services;
        }
    }
}
