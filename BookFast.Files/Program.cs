using Microsoft.Diagnostics.EventFlow.ServiceFabric;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;

namespace BookFast.Files
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                using (var terminationEvent = new ManualResetEvent(initialState: false))
                {
                    using (var diagnosticsPipeline = ServiceFabricDiagnosticPipelineFactory.CreatePipeline("BookFast-Files-DiagnosticsPipeline"))
                    {
                        Console.CancelKeyPress += (sender, eventArgs) => Shutdown(diagnosticsPipeline, terminationEvent);

                        AppDomain.CurrentDomain.UnhandledException += (sender, unhandledExceptionArgs) =>
                        {
                            ServiceEventSource.Current.UnhandledException(unhandledExceptionArgs.ExceptionObject?.ToString() ?? "(no exception information)");
                            Shutdown(diagnosticsPipeline, terminationEvent);
                        };

                        ServiceRuntime.RegisterServiceAsync("FilesServiceType",
                                    context => new FilesService(context)).GetAwaiter().GetResult();

                        ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(FilesService).Name);

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
    }
}
