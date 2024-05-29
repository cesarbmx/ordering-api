using CesarBmx.Shared.Api.Configuration;


namespace CesarBmx.Ordering.Api.Configuration
{
    public static class RedisConfig
    {
        public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSharedRedis(configuration);
            //services.AddSharedCache(configuration, "LookupApi");

            return services;
        }
    }
}