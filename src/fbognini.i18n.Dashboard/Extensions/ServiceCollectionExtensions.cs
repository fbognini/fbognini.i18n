﻿using AutoMapper;
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
        public static IServiceCollection AddI18nDashboardServices(this IServiceCollection services)
        {
            services.AddControllersWithViews();

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
