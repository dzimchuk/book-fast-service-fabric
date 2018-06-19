using BookFast.Security;
using BookFast.SeedWork;
using BookFast.SeedWork.CommandStack;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents
{
    internal class ReliableEventsDispatcher : INotificationHandler<EventsAvailableNotification>
    {
        private readonly IReliableEventsDataSource dataSource;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
                
        private readonly AutoResetEvent dispatcherTrigger = new AutoResetEvent(false);

        private CancellationTokenSource cancellationTokenSource;
        private Timer timer;

        public ReliableEventsDispatcher(IReliableEventsDataSource dataSource, ILogger<ReliableEventsDispatcher> logger, IServiceProvider serviceProvider)
        {
            this.dataSource = dataSource;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public Task RunDispatcherAsync(CancellationToken cancellationToken)
        {
            dispatcherTrigger.Reset();

            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => WaitAndProcessEventsAsync(cancellationTokenSource.Token));

            var interval = TimeSpan.FromMinutes(2);
            timer = new Timer(state =>
            {
                dispatcherTrigger.Set();
            }, null, interval, interval);
        }

        public void StopDispatcher()
        {
            timer.Dispose();
            cancellationTokenSource.Cancel();
            dispatcherTrigger.Set();
        }

        public Task Handle(EventsAvailableNotification notification, CancellationToken cancellationToken)
        {
            dispatcherTrigger.Set();
            return Task.CompletedTask;
        }

        private async Task WaitAndProcessEventsAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                dispatcherTrigger.WaitOne();

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var events = await dataSource.GetPendingEventsAsync(cancellationToken);
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
    }
}
