using CesarBmx.Notification.Api.Configuration;
using CesarBmx.Notification.Application.Settings;
using CesarBmx.Shared.Api.Configuration;

namespace CesarBmx.Notification.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Settings
            services.ConfigureSettings(Configuration);

            // CORS
            services.ConfigureCors(Configuration);

            // Automapper
            services.ConfigureAutomapper();

            // Swagger
            services.ConfigureSwagger();

            // DI
            services.ConfigureDependencies(Configuration);

            // Data migration
            services.ConfigureDataSeeding();

            // Hangfire
            services.ConfigureHangfire(Configuration);

            // Elmah
            services.ConfigureElmah();

            // Authentication
            services.ConfigureAuthentication(Configuration);

            // Authorization
            services.ConfigureAuthorization();

            // Distributed caching
            services.ConfigureCaching();

            // Mvc
            services.ConfigureMvc(Configuration);

            // Cors
            services.ConfigureCors(Configuration);

            // Health
            services.ConfigureHealth(Configuration);

            // Fluent validation
            services.ConfigureFluentValidation();

            // Open telemetry
            services.ConfigureOpenTelemetry(Configuration);

            // Masstransit
            services.ConfigureMasstransit(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            // Grab settings
            var appSettings = Configuration.GetSection<AppSettings>();

            // This for the reverse proxy
            app.UsePathBase("/" + appSettings.PathBase);

            // CORS
            app.ConfigureCors();

            // Error handling
            app.ConfigureErrorHandling();

            // Serilog
            app.ConfigureSerilog(loggerFactory, Configuration);

            // Swagger
            app.ConfigureSwagger(Configuration);

            // Hangfire
            app.ConfigureHangfire(Configuration);

            // Elmah
            app.ConfigureElmah(Configuration);

            // Mvc
            app.ConfigureMvc(Configuration);

            // Health
            app.ConfigureHealth();

            // Open telemetry
            app.ConfigureOpenTelemetry();

            // Masstransit
            app.ConfigureMasstransit();
        }
    }
}
