using CesarBmx.Ordering.Application.Jobs;
using CesarBmx.Ordering.Application.Services;
using CesarBmx.Ordering.Application.Settings;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Shared.Api.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();

            //Db contexts
            if (appSettings.UseMemoryStorage)
            {
                services.AddDbContext<MainDbContext, MainDbContext>(options => options
                     .UseInMemoryDatabase(appSettings.DatabaseName)
                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }
            else
            {
                services.AddDbContext<MainDbContext, MainDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString(appSettings.DatabaseName))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }

            // Services
            services.AddScoped<OrderService>();

            // Jobs
            services.AddScoped<MainJob>();
            services.AddScoped<SendWhatsappNotificationsJob>();
            services.AddScoped<SendTelgramNotificationsJob>();

            // API clients
            services.AddScoped<CoinpaprikaAPI.Client, CoinpaprikaAPI.Client>();

            // Open telemetry
            services.AddSingleton(x=> new ActivitySource(appSettings.ApplicationId));

            // Return
            return services;
        }
    }
}
