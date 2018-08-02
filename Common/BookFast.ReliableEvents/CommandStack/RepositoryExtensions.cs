using BookFast.Security;
using BookFast.SeedWork.Modeling;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents.CommandStack
{
    public static class RepositoryExtensions
    {
        public static async Task SaveChangesAsync<TEntity>(this IRepositoryWithReliableEvents<TEntity> repository, TEntity entity, CommandContext context) 
            where TEntity : IAggregateRoot, IEntity
        {
            var isOwner = context.AcquireOwnership();

            var events = entity.CollectEvents() ?? new List<Event>();

            var integrationEvents = events.OfType<IntegrationEvent>().ToList();
            if (integrationEvents.Any())
            {
                await repository.PersistEventsAsync(integrationEvents.AsReliableEvents(context.SecurityContext));
                context.NotifyWhenDone();
            }
            
            foreach (var @event in events.Except(integrationEvents).OrderBy(evt => evt.OccurredAt))
            {
                await context.Mediator.Publish(@event);
            }

            if (isOwner)
            {
                await repository.SaveChangesAsync();

                if (context.ShouldNotify)
                {
                    try
                    {
                        await context.Mediator.Publish(new EventsAvailableNotification());
                    }
                    catch (Exception ex)
                    {
                        context.Logger.LogError($"Error sending event notification. Details: {ex}");
                    }
                }
            }
        }

        private static IEnumerable<ReliableEvent> AsReliableEvents(this IEnumerable<IntegrationEvent> events, ISecurityContext securityContext)
        {
            return from @event in events
                   select new ReliableEvent
                   {
                       Id = @event.EventId.ToString(),
                       EventName = @event.GetType().Name,
                       OccurredAt = @event.OccurredAt,
                       User = securityContext.GetCurrentUser(),
                       Tenant = securityContext.GetCurrentTenant(),
                       Payload = JsonConvert.SerializeObject(@event)
                   };
        }
    }
}
