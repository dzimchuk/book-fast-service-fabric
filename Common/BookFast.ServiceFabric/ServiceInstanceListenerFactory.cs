using Microsoft.AspNetCore.Hosting;
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
        public static ServiceInstanceListener CreateListener(Type startupType, Action<ServiceContext, string> loggingCallback)
        {
            return new ServiceInstanceListener(serviceContext =>
            {
                var config = serviceContext.CodePackageActivationContext.GetConfigurationPackageObject("Config");
                var environment = config.Settings.Sections["Environment"].Parameters["ASPNETCORE_ENVIRONMENT"].Value;

                return new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", url =>
                {
                    loggingCallback(serviceContext, $"Starting Kestrel on {url}");

                    return new WebHostBuilder().UseKestrel()
                                .ConfigureServices(
                                    services => services
                                        .AddSingleton<StatelessServiceContext>(serviceContext))
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup(startupType)
                                .UseEnvironment(environment)
                                .UseUrls(url)
                                .Build();
                });
            });
        }
    }
}
