using CesarBmx.Ordering.Application.Mappers;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class AutomapperConfig
    {
        public static IServiceCollection ConfigureAutomapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(OrderMapper).Assembly);

            return services;
        }
    }
}
