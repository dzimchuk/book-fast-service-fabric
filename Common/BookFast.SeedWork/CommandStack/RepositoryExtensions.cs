using BookFast.SeedWork.Modeling;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.SeedWork.CommandStack
{
    public static class RepositoryExtensions
    {
        public static async Task SaveChangesAsync<TEntity>(this IRepository<TEntity> repository, TEntity entity, CommandContext context) 
            where TEntity : IAggregateRoot, IEntity
        {
            var isOwner = context.AcquireOwnership();

            var events = entity.CollectEvents()?.ToList();

            var integrationEvents = events.OfType<IntegrationEvent>().ToList();
            if (integrationEvents.Any())
            {
                await repository.PersistEventsAsync(integrationEvents);
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
                    await context.Mediator.Publish(new EventsAvailableNotification());
                }
            }
        }
    }
}
