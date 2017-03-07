using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
        public static ServiceReplicaListener CreateHttpSysListener(Type startupType, IReliableStateManager stateManager, Action<StatefulServiceContext, string> loggingCallback)
        {
            return new ServiceReplicaListener(serviceContext =>
            {
                return new WebListenerCommunicationListener(serviceContext, "ServiceEndpoint", url =>
                {
                    loggingCallback(serviceContext, $"Starting WebListener on {url}");

                    return new WebHostBuilder().UseWebListener()
                                .ConfigureServices(services =>
                                {
                                    services.AddSingleton(serviceContext);
                                    services.AddSingleton(stateManager);
                                })
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup(startupType)
                                .UseUrls(url)
                                .Build();
                });
            });
        }
    }
}
