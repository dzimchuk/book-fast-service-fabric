using BookFast.ServiceFabric.AppInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Fabric;
using System.IO;

namespace BookFast.ServiceFabric
{
    public static class ServiceInstanceListenerFactory
    {
        public static ServiceInstanceListener CreateInternalListener(Type startupType, Action<StatelessServiceContext, string> loggingCallback)
        {
            return new ServiceInstanceListener(serviceContext =>
            {
                return new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                {
                    loggingCallback(serviceContext, $"Starting Kestrel on {url}");

                    return new WebHostBuilder().UseKestrel()
                                .ConfigureServices((hostingContext, services) =>
                                {
                                    services.AddSingleton<StatelessServiceContext>(serviceContext);

                                    services.AddApplicationInsightsTelemetry(hostingContext.Configuration);
                                    services.AddSingleton<ITelemetryInitializer>((serviceProvider) => new FabricTelemetryInitializer(serviceContext));
                                })
                                .ConfigureAppConfiguration((hostingContext, config) =>
                                {
                                    config.AddServiceFabricConfiguration(serviceContext);
                                })
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.UseUniqueServiceUrl)
                                .UseStartup(startupType)
                                .UseUrls(url)
                                .Build();
                });
            });
        }

        public static ServiceInstanceListener CreateExternalListener(Type startupType, Action<StatelessServiceContext, string> loggingCallback)
        {
            return new ServiceInstanceListener(serviceContext =>
            {
                return new HttpSysCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                {
                    loggingCallback(serviceContext, $"Starting HttpSys listener on {url}");

                    return new WebHostBuilder().UseHttpSys()
                                .ConfigureServices((hostingContext, services) =>
                                {
                                    services.AddSingleton<StatelessServiceContext>(serviceContext);

                                    services.AddApplicationInsightsTelemetry(hostingContext.Configuration);
                                    services.AddSingleton<ITelemetryInitializer>((serviceProvider) => new FabricTelemetryInitializer(serviceContext));
                                })
                                .ConfigureAppConfiguration((hostingContext, config) =>
                                {
                                    config.AddServiceFabricConfiguration(serviceContext);
                                })
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                .UseStartup(startupType)
                                .UseUrls(url)
                                .Build();
                });
            });
        }
    }
}
