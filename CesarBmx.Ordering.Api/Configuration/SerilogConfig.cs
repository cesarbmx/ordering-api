using CesarBmx.Shared.Api.Configuration;
using System.Reflection;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class SerilogConfig
    {
        public static ILoggerFactory ConfigureSerilog(this ILoggerFactory logger, IConfiguration configuration)
        {
            logger.ConfigureSharedSerilog(configuration, Assembly.GetExecutingAssembly());

            return logger;
        }
    }
}
