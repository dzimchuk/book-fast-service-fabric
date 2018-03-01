using BookFast.ServiceFabric.AppInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Fabric;
using System.IO;

namespace BookFast.ServiceFabric
{
    public static class ServiceReplicaListenerFactory
    {
        public static ServiceReplicaListener CreateListener(Type startupType, IReliableStateManager stateManager, Action<StatefulServiceContext, string> loggingCallback)
        {
            return new ServiceReplicaListener(serviceContext =>
            {
                return new KestrelCommunicationListener(serviceContext, (url, listener) =>
                {
                    loggingCallback(serviceContext, $"Starting Kestrel on {url}");

                    return new WebHostBuilder().UseKestrel()
                                .ConfigureServices((hostingContext, services) =>
                                {
                                    services.AddSingleton(serviceContext);
                                    services.AddSingleton(stateManager);

                                    services.AddApplicationInsightsTelemetry(hostingContext.Configuration);
                                    services.AddSingleton<ITelemetryInitializer>((serviceProvider) => new FabricTelemetryInitializer(serviceContext));
                                })
                                .ConfigureAppConfiguration((hostingContext, config) =>
                                {
                                    config.AddServiceFabricConfiguration(serviceContext);
                                })
                                .ConfigureLogging((hostingContext, logging) =>
                                {
                                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                    logging.AddDebug();
                                })
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.UseUniqueServiceUrl)
                                .UseStartup(startupType)
                                .UseUrls(url)
                                .Build();
                });
            });
        }
    }
}
