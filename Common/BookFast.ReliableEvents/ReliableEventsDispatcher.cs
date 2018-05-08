using BookFast.Security;
using BookFast.SeedWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents
{
    internal class ReliableEventsDispatcher : IHostedService
    {
        private readonly IReliableEventsDataSource dataSource;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public ReliableEventsDispatcher(IReliableEventsDataSource dataSource, ILogger<ReliableEventsDispatcher> logger, IServiceProvider serviceProvider)
        {
            this.dataSource = dataSource;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
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

                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
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

                        var okToClear = await PublishEventAsync(@event, cancellationToken);
                        if (okToClear)
                        {
                            await dataSource.ClearEventAsync(@event.Id, cancellationToken); 
                        }
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
            }
        }

        private async Task<bool> PublishEventAsync(ReliableEvent @event, CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var securityContext = scope.ServiceProvider.GetRequiredService<ISecurityContext>();

                InitializeSecurityContext(@event, securityContext);

                try
                {
                    await mediator.Publish(@event.Event, cancellationToken);
                    return true;
                }
                catch (BusinessException ex)
                {
                    logger.LogError($"Business exception occurred while processing reliable event. Details: {ex}");
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Unknown error occured while processing reliable event. Details: {ex}");
                    return false; // TODO: handle poison events
                }
            }
        }

        private static void InitializeSecurityContext(ReliableEvent @event, ISecurityContext securityContext)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(BookFastClaimTypes.ObjectId, @event.User));
            identity.AddClaim(new Claim(BookFastClaimTypes.TenantId, @event.Tenant));

            var acceptor = (ISecurityContextAcceptor)securityContext;
            acceptor.Principal = new ClaimsPrincipal(identity);
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
