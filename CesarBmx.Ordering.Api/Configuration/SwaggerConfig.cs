using CesarBmx.Shared.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CesarBmx.Ordering.Api.ResponseExamples;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.ConfigureSharedSwagger("Notification API", typeof(MessageResponseExample));

            return services;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, IConfiguration config)
        {
            app.ConfigureSharedSwagger("Notification API");

            return app;
        }
    }
}
