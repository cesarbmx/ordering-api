using CesarBmx.Shared.Api.Configuration;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class MvcConfig
    {
        public static IServiceCollection ConfigureMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSharedMvc(configuration);
            
            return services;
        }

        public static IApplicationBuilder ConfigureMvc(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.ConfigureSharedMvc(configuration);

            return app;
        }
    }
}
