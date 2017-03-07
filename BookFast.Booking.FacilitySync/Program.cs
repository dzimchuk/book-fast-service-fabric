using BookFast.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Threading;

namespace BookFast.Booking.FacilitySync
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("FacilitySyncServiceType",
                    context => CreateServiceInstance(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(FacilitySyncService).Name);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static FacilitySyncService CreateServiceInstance(StatelessServiceContext context)
        {
            var builder = new ConfigurationBuilder()
                       .AddServiceFabricConfiguration(context);

            var configuration = builder.Build();
            var serviceProvider = GetServiceProvider(configuration, context);

            return serviceProvider.GetService<FacilitySyncService>();
        }

        private static IServiceProvider GetServiceProvider(IConfigurationRoot configuration, StatelessServiceContext context)
        {
            var services = new ServiceCollection();
            var modules = new List<ICompositionModule>
                          {
                              new Composition.CompositionModule()
                          };

            foreach (var module in modules)
            {
                module.AddServices(services, configuration);
            }
                        
            services.AddSingleton(context);

            return services.BuildServiceProvider();
        }
    }
}
