using BookFast.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Threading;

namespace BookFast.Booking
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("BookingServiceType",
                    context => CreateServiceInstance(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(BookingService).Name);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static BookingService CreateServiceInstance(StatefulServiceContext context)
        {
            var builder = new ConfigurationBuilder()
                       .AddServiceFabricConfiguration(context);

            var configuration = builder.Build();
            var serviceProvider = GetServiceProvider(configuration, context);

            return serviceProvider.GetService<BookingService>();
        }

        private static IServiceProvider GetServiceProvider(IConfigurationRoot configuration, StatefulServiceContext context)
        {
            var services = new ServiceCollection();
            var modules = new List<ICompositionModule>
                          {
                              new Business.Composition.CompositionModule(),
                              new Data.Composition.CompositionModule()
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
