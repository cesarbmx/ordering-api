﻿using CesarBmx.Shared.Api.Configuration;


namespace CesarBmx.Ordering.Api.Configuration
{
    public static class ElasticConfig
    {
        public static IServiceCollection ConfigureElastic(this IServiceCollection services)
        {
            services.ConfigureSharedElastic();

            return services;
        }
    }
}
