using AutoMapper;
using fbognini.AutoMapper.Extensions;
using fbognini.WebFramework.Handlers;
using FluentValidation;
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
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddI18nDashboardServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddRazorPages();

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
