﻿using fbognini.i18n.Localizers;
using fbognini.i18n.Persistence;
using fbognini.i18n.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace fbognini.i18n
{
    public static class Startup
    {
        public static IServiceCollection AddI18N(this IServiceCollection services, Action<I18nSettings> options)
        {
            var settings = new I18nSettings();
            options.Invoke(settings);

            services.Configure(options);

            return services.AddI18N(settings);
        }

        public static IServiceCollection AddI18N(this IServiceCollection services, IConfiguration configuration, Action<I18nSettings>? options = null)
        {
            return services.AddI18N(configuration.GetSection(nameof(I18nSettings)), options);
        }

        public static IServiceCollection AddI18N(this IServiceCollection services, IConfigurationSection section, Action<I18nSettings>? options = null)
        {
            var settings = section.Get<I18nSettings>() ?? new I18nSettings();

            services.Configure<I18nSettings>(section);

            if (options != null)
            {
                options(settings);
                services.PostConfigure<I18nSettings>(setting =>
                {
                    options(setting);
                });
            }

            return services.AddI18N(settings);
        }

        public static async Task InitializeI18N(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<I18nContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
        }

        public static async Task<IApplicationBuilder> InitializeI18N(this IApplicationBuilder app, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<I18nContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }

            return app;
        }

        public static IApplicationBuilder UseRequestLocalizationI18N(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<II18nRepository>();
            var languages = service.GetLanguages();
            languages = languages.Where(x => x.IsActive).ToList();

            if (languages.Any())
            {
                var defaultCulture = languages.FirstOrDefault(x => x.IsDefault);
                if (defaultCulture == null)
                {
                    defaultCulture = languages.First();
                }

                var options = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
                    .SetDefaultCulture(defaultCulture.Id)
                    .AddSupportedCultures(languages.Select(x => x.Id).ToArray())
                    .AddSupportedUICultures(languages.Select(x => x.Id).ToArray());

                app.UseRequestLocalization(options);
            }

            return app;
        }

        private static IServiceCollection AddI18N(this IServiceCollection services, I18nSettings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.CookieName))
            {
                var provider = new CookieRequestCultureProvider()
                {
                    CookieName = settings.CookieName
                };
                services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.RequestCultureProviders.Insert(0, provider);
                });
            }

            services.AddLocalization();

            if (settings.UseCache)
            {
                services.AddMemoryCache();
                services.AddSingleton<II18nRepository, I18nCachedRepository>();
            }
            else
            {
                services.AddSingleton<II18nRepository, I18nRepository>();
            }

            services.AddTransient<TranslateResolver>();

            services.AddTransient<LocalizedPathResolver>();
            services.AddTransient<NotLocalizedPathResolver>();

            services.AddTransient<ImagesLocalizedPathResolver>();
            services.AddTransient<ImageNotLocalizedPathResolver>();
            services.AddDbContext<I18nContext>(options =>
                options.UseSqlServer(settings.ConnectionString),
                    ServiceLifetime.Singleton,
                    ServiceLifetime.Singleton);

            services.AddSingleton<IStringLocalizerFactory, EFStringLocalizerFactory>();
            services.AddSingleton<IExtendedStringLocalizerFactory, EFStringLocalizerFactory>();

            return services;
        }
    }
}
