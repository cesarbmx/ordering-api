using CesarBmx.Shared.Api.Configuration;
using CesarBmx.Ordering.Application.Settings;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class SettingsConfig
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<AppSettings>(configuration);
            services.ConfigureSharedSettings(configuration);

            return services;
        }
    }
}