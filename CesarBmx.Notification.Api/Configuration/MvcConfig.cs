using CesarBmx.Shared.Api.Configuration;

namespace CesarBmx.Notification.Api.Configuration
{
    public static class MvcConfig
    {
        public static IServiceCollection ConfigureMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSharedMvc(configuration, false);
            
            return services;
        }

        public static IApplicationBuilder ConfigureMvc(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.ConfigureSharedMvc(configuration, false);

            return app;
        }
    }
}
