using BookFast.ReliableEvents.DistributedMutex;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents
{
    internal class ReliableEventsDispatcherService : BackgroundService
    {
        private readonly ReliableEventsDispatcher dispatcher;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public ReliableEventsDispatcherService(ReliableEventsDispatcher dispatcher, 
            IConfiguration configuration, 
            ILogger<ReliableEventsDispatcherService> logger)
        {
            this.dispatcher = dispatcher;
            this.configuration = configuration;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration["Data:Azure:Storage:ConnectionString"]);
            var settings = new BlobSettings(storageAccount, "mutex", configuration["General:ServiceName"]);
            var mutex = new BlobDistributedMutex(settings, dispatcher.RunDispatcherAsync, logger);

            while (!stoppingToken.IsCancellationRequested)
            {
                await mutex.RunTaskWhenMutexAcquired(stoppingToken);
            }
        }
    }
}
