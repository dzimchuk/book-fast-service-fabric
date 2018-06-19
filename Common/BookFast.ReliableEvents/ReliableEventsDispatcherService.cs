using BookFast.ReliableEvents.DistributedMutex;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents
{
    internal class ReliableEventsDispatcherService : IHostedService
    {
        private readonly ReliableEventsDispatcher dispatcher;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        private readonly CancellationTokenSource globalCts = new CancellationTokenSource();
        
        public ReliableEventsDispatcherService(ReliableEventsDispatcher dispatcher, IConfiguration configuration, ILogger<ReliableEventsDispatcherService> logger)
        {
            this.dispatcher = dispatcher;
            this.configuration = configuration;
            this.logger = logger;
        }

        private async Task RunDispatcherAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(configuration["Data:Azure:Storage:ConnectionString"]);
            var settings = new BlobSettings(storageAccount, "mutex", "blob");
            var mutex = new BlobDistributedMutex(settings, dispatcher.RunDispatcherAsync, logger);

            while (!globalCts.IsCancellationRequested)
            {
                await mutex.RunTaskWhenMutexAcquired(globalCts.Token);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => RunDispatcherAsync());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            globalCts.Cancel();
            return Task.CompletedTask;
        }
    }
}
