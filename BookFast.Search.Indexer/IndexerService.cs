using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.Extensions.Hosting;

namespace BookFast.Search.Indexer
{
    internal sealed class IndexerService : StatelessService
    {
        private readonly IHostedService[] hostedServices;

        public IndexerService(StatelessServiceContext context, IHostedService[] hostedServices)
            : base(context)
        {
            this.hostedServices = hostedServices;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            foreach (var service in hostedServices)
            {
                await service.StartAsync(cancellationToken);
            }

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    foreach (var service in hostedServices)
                    {
                        await service.StopAsync(CancellationToken.None);
                    }

                    return;
                }

                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}