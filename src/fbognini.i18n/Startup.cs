using fbognini.i18n.Persistence;
using fbognini.i18n.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection AddI18N(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("i18n").Get<Settings>();
            services.AddSingleton(settings.Context);

            if (settings.UseCache)
            {
                services.AddMemoryCache();
                services.AddScoped<II18nRepository, I18nCachedRepository>();
            }
            else 
            { 
                services.AddScoped<II18nRepository, I18nRepository>();
            }

            services.AddTransient<TranslateResolver>();

            services.AddTransient<LocalizedPathResolver>();
            services.AddTransient<NotLocalizedPathResolver>();

            services.AddTransient<ImageAllLocalizedPathResolver>();
            services.AddTransient<ImageNotLocalizedPathResolver>();
            services.AddDbContext<I18nContext>(options => options
                    .UseSqlServer(settings.ConnectionString));

            return services;
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
        public static async Task InitializeI18N(this IApplicationBuilder app, IConfiguration configuration, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<I18nContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }

            var settings = configuration.GetSection("i18n").Get<Settings>();
            if (!settings.UseRequestLocalization)
                return;

            var repository = scope.ServiceProvider.GetRequiredService<II18nRepository>();
            var languages = await repository.GetLanguages(cancellationToken);
            languages = languages.Where(x => x.IsActive).ToList();

            if (languages.Any())
            {
                var defaultCulture = languages.FirstOrDefault(x => x.IsDefault);
                if (defaultCulture == null)
                {
                    defaultCulture = languages.First();
                }

                var options = new RequestLocalizationOptions()
                    .SetDefaultCulture(defaultCulture.Id)
                    .AddSupportedCultures(languages.Select(x => x.Id).ToArray());

                app.UseRequestLocalization(options);
            }
        }

    }
}
