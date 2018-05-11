using BookFast.Security;
using BookFast.SeedWork;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ServiceBus
{
    internal class IntegrationEventReceiver : IHostedService
    {
        private readonly IEventMapper eventMapper;
        private readonly ConnectionOptions options;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        private SubscriptionClient client;

        public IntegrationEventReceiver(IEventMapper eventMapper,
            IOptions<ConnectionOptions> options,
            ILogger<IntegrationEventReceiver> logger, 
            IServiceProvider serviceProvider)
        {
            this.eventMapper = eventMapper;
            this.options = options.Value;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                client = new SubscriptionClient(options.ConnectionString, options.TopicName, options.SubscriptionName, ReceiveMode.PeekLock, RetryPolicy.Default);
                
                client.RegisterMessageHandler(
                    OnMessageAsync,
                    new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false }); 
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (client != null)
            {
                await client.CloseAsync();
            }
        }

        private async Task OnMessageAsync(Message message, CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var securityContext = scope.ServiceProvider.GetService<ISecurityContext>();
                if (securityContext != null)
                {
                    InitializeSecurityContext(message.UserProperties, securityContext);
                }

                await OnMessageAsync(message, cancellationToken, mediator); 
            }
        }

        private async Task OnMessageAsync(Message message, CancellationToken cancellationToken, IMediator mediator)
        {
            try
            {
                var payload = JObject.Parse(Encoding.UTF8.GetString(message.Body));
                var command = MapEvent(message.Label, payload);
                if (command != null)
                {
                    if (command is IRequest)
                    {
                        await mediator.Send((IRequest)command, cancellationToken);
                    }
                    else if (command is IRequest<int>)
                    {
                        await mediator.Send((IRequest<int>)command, cancellationToken);
                    }
                    else
                    {
                        logger.LogWarning($"Unsupported command type: {command.GetType()}");
                    }
                }

                await client.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (BusinessException ex)
            {
                logger.LogError($"Business exception occurred processing integration event. Details: {ex}");
                await client.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected error occurred processing integration event. Details: {ex}");
                await client.AbandonAsync(message.SystemProperties.LockToken);
            }
        }

        private IBaseRequest MapEvent(string eventName, JObject payload)
        {
            try
            {
                return eventMapper.MapEvent(eventName, payload);
            }
            catch (Exception ex)
            {
                throw new EventMapperException(ex);
            }
        }

        private static void InitializeSecurityContext(IDictionary<string, object> userProperties, ISecurityContext securityContext)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(BookFastClaimTypes.ObjectId, userProperties[Constants.ObjectId].ToString()));
            identity.AddClaim(new Claim(BookFastClaimTypes.TenantId, userProperties[Constants.TenantId].ToString()));

            var acceptor = (ISecurityContextAcceptor)securityContext;
            acceptor.Principal = new ClaimsPrincipal(identity);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            logger.LogError($"IntegrationEventReceiver encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }
    }
}
