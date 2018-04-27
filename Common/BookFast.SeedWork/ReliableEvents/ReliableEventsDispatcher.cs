using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.SeedWork.ReliableEvents
{
    internal class ReliableEventsDispatcher : IHostedService
    {
        private readonly IReliableEventsDataSource dataSource;
        private readonly ILogger logger;
        private readonly IMediator mediator;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public ReliableEventsDispatcher(IReliableEventsDataSource dataSource, ILogger<ReliableEventsDispatcher> logger, IMediator mediator)
        {
            this.dataSource = dataSource;
            this.logger = logger;
            this.mediator = mediator;
        }

        private async Task<IEnumerable<ReliableEvent>> FetchPendingEventsAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                var events = await dataSource.GetPendingEventsAsync(cancellationToken);
                if (events != null && events.Any())
                {
                    return events;
                }

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        private async Task RunDispatcherAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    var events = await FetchPendingEventsAsync(cancellationToken);
                    foreach (var @event in events)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        await mediator.Publish(@event.Event, cancellationToken);
                        await dataSource.ClearEventAsync(@event.Id, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("Cancellation request received in ReliableEventsDispatcher.");
                    return;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error processing reliable events. Details: {ex}");
                }

                // TODO: handle poison events
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => RunDispatcherAsync(cancellationTokenSource.Token));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
