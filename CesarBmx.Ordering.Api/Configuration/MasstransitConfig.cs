using CesarBmx.Shared.Api.Configuration;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Ordering.Application.Consumers;
using CesarBmx.Ordering.Application.Sagas;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class MasstransitConfig
    {
        public static IServiceCollection ConfigureMasstransit(this IServiceCollection services, IConfiguration configuration)
        {
            // Shared
            services.ConfigureSharedMasstransit<MainDbContext>(configuration, typeof(OrderPlacedConsumer), typeof(OrderSaga));

            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureMasstransit(this IApplicationBuilder app)
        {
            // Shared
            app.ConfigureSharedMasstransit();

            return app;
        }
    }
}
