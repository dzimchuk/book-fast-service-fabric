using BookFast.SeedWork;
using Microsoft.Diagnostics.EventFlow.ServiceFabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Threading;

namespace BookFast.Search.Indexer
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                using (var terminationEvent = new ManualResetEvent(initialState: false))
                {
                    using (var diagnosticsPipeline = ServiceFabricDiagnosticPipelineFactory.CreatePipeline("BookFast-SearchIndexer-DiagnosticsPipeline"))
                    {
                        Console.CancelKeyPress += (sender, eventArgs) => Shutdown(diagnosticsPipeline, terminationEvent);

                        AppDomain.CurrentDomain.UnhandledException += (sender, unhandledExceptionArgs) =>
                        {
                            ServiceEventSource.Current.UnhandledException(unhandledExceptionArgs.ExceptionObject?.ToString() ?? "(no exception information)");
                            Shutdown(diagnosticsPipeline, terminationEvent);
                        };

                        ServiceRuntime.RegisterServiceAsync("IndexerServiceType",
                                    context => CreateServiceInstance(context)).GetAwaiter().GetResult();

                        ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(IndexerService).Name);

                        terminationEvent.WaitOne();
                    } 
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static void Shutdown(IDisposable disposable, ManualResetEvent terminationEvent)
        {
            try
            {
                disposable.Dispose();
            }
            finally
            {
                terminationEvent.Set();
            }
        }

        private static IndexerService CreateServiceInstance(StatelessServiceContext context)
        {
            var builder = new ConfigurationBuilder()
                       .AddServiceFabricConfiguration(context);

            var configuration = builder.Build();
            var serviceProvider = GetServiceProvider(configuration, context);

            return serviceProvider.GetService<IndexerService>();
        }

        private static IServiceProvider GetServiceProvider(IConfigurationRoot configuration, StatelessServiceContext context)
        {
            var services = new ServiceCollection();
            var modules = new List<ICompositionModule>
                          {
                              new Composition.CompositionModule(),
                              new Adapter.Composition.CompositionModule()
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
