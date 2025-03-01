using Serilog;

namespace MyBaseProject.API.Extensions
{
    public static class LoggingExtensions
    {
        public static void AddLoggingConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, config) =>
            {
                config.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
            });
        }
    }
}
