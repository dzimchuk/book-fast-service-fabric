using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class LoggingExtensions
    {
        public static void AddAppInsights(this ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
            var logLevel = configuration["Logging:LogLevel:Default"] ?? "Warning";

            loggerFactory.AddApplicationInsights(serviceProvider, (LogLevel)Enum.Parse(typeof(LogLevel), logLevel, true));
        }
    }
}
