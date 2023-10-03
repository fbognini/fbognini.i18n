using AutoMapper;
using fbognini.AutoMapper.Extensions;
using fbognini.i18n.Dashboard.Authorization;
using fbognini.WebFramework.Handlers;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace fbognini.i18n.Dashboard.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddI18nDashboardServices(this IServiceCollection services, bool authorize)
        {
            services.AddRazorPages(options =>
            {
                if (authorize)
                {
                    options.Conventions.AuthorizeAreaFolder("i18n", "/", I18nDashboardPolicy.Dashboard);
                }
            });

            services.AddAutoMapper(delegate (IMapperConfigurationExpression config)
            {
                config.AddCustomMappingProfile<IMarker>();
            });
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
