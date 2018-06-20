using BookFast.Security;
using BookFast.SeedWork;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents
{
    internal class ReliableEventsDispatcher
    {
        private const int PeriodicCheckIntervalInMinutes = 2;

        private readonly IReliableEventsDataSource dataSource;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        private readonly ConnectionOptions serviceBusConnectionOptions;


        private readonly AutoResetEvent dispatcherTrigger = new AutoResetEvent(false);

        public ReliableEventsDispatcher(IReliableEventsDataSource dataSource, 
            ILogger<ReliableEventsDispatcher> logger, 
            IServiceProvider serviceProvider, 
            IOptions<ConnectionOptions> serviceBusConnectionOptions)
        {
            this.dataSource = dataSource;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.serviceBusConnectionOptions = serviceBusConnectionOptions.Value;
        }

        public async Task RunDispatcherAsync(CancellationToken cancellationToken)
        {
            dispatcherTrigger.Reset();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(() => WaitAndProcessEventsAsync(cancellationToken));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            var interval = TimeSpan.FromMinutes(PeriodicCheckIntervalInMinutes);
            var timer = new Timer(state =>
            {
                dispatcherTrigger.Set();
            }, null, interval, interval);

            var queueClient = StartNotificationReceiver();

            await WaitCancellationAsync(cancellationToken);

            timer.Dispose();
            dispatcherTrigger.Set();
            await queueClient.CloseAsync();
        }

        private static async Task WaitCancellationAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(-1), cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private QueueClient StartNotificationReceiver()
        {
            var queueClient = new QueueClient(serviceBusConnectionOptions.NotificationQueueConnection, serviceBusConnectionOptions.NotificationQueueName, ReceiveMode.ReceiveAndDelete, RetryPolicy.Default);
            queueClient.RegisterMessageHandler((message, cancellationToken) =>
            {
                dispatcherTrigger.Set();
                return Task.CompletedTask;
            }, new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 1 });

            return queueClient;
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            logger.LogError($"Error receiving reliable events notification. Details: {arg.Exception}.");
            return Task.CompletedTask;
        }

        private async Task WaitAndProcessEventsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
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

                dispatcherTrigger.WaitOne();
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
