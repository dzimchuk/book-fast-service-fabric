using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Fabric;

namespace BookFast.ServiceFabric
{
    public static class ApplicationInsightsExtensions
    {
        public static void AddAppInsights(this IServiceCollection services, IConfiguration configuration, ServiceContext serviceContext)
        {
            services.AddApplicationInsightsTelemetry(configuration);

            //var performanceCounterService = services.FirstOrDefault(t => t.ImplementationType == typeof(PerformanceCollectorModule));
            //if (performanceCounterService != null)
            //{
            //    services.Remove(performanceCounterService);
            //}

            //services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
            //    FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(serviceContext));
        }

        public static void AddAppInsights(this ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddApplicationInsights(serviceProvider, LogLevel.Warning);
        }
    }
}
