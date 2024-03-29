﻿using CesarBmx.Shared.Api.Configuration;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Ordering.Application.Consumers;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class MasstransitConfig
    {
        public static IServiceCollection ConfigureMasstransit(this IServiceCollection services, IConfiguration configuration)
        {
            // Shared
            //services.ConfigureSharedMasstransit<MainDbContext>(configuration, typeof(PlaceOrderConsumer), typeof(OrderSaga));
            services.ConfigureSharedMasstransit<MainDbContext>(configuration, typeof(PlaceOrderConsumer));

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
