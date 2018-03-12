using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.SeedWork.Modeling
{
    public abstract class Entity<TIdentity>
    {
        private TIdentity id = default(TIdentity);
        private List<Event> events;

        protected Entity()
        {
        }

        public TIdentity Id
        {
            get { return id; }
            set
            {
                if (id.Equals(default(TIdentity)))
                {
                    throw new InvalidOperationException("Entity ID cannot be changed");
                }

                id = value;
            }
        }

        public void AddEvent(Event @event)
        {
            if (events == null)
            {
                events = new List<Event>();
            }

            events.Add(@event);
        }

        public async Task RaiseEventsAsync(IMediator mediator)
        {
            var events = CollectEvents();
            if (events != null && events.Any())
            {
                var tasks = events
                            .OrderBy(@event => @event.OccurredAt)
                            .Select(@event => mediator.Publish(@event));

                await Task.WhenAll(tasks);
            }
        }

        protected virtual IEnumerable<Event> CollectEvents() => events;
    }
}
