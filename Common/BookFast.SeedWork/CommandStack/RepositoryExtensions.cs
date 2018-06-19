using BookFast.SeedWork.Modeling;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.SeedWork.CommandStack
{
    public static class RepositoryExtensions
    {
        public static async Task SaveChangesAsync(this IRepository repository, IEntity entity, CommandContext context)
        {
            var events = entity.CollectEvents().ToList();

            var integrationEvents = events.OfType<IntegrationEvent>().ToList();
            if (integrationEvents.Any())
            {
                await repository.PersistEventsAsync(integrationEvents);
                context.EventsAvailable = true;
            }

            await repository.SaveChangesAsync();

            foreach (var @event in events.Except(integrationEvents).OrderBy(evt => evt.OccurredAt))
            {
                await context.Mediator.Publish(@event);
            }
        }
    }
}
