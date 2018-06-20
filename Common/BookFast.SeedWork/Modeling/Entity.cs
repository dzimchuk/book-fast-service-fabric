using System;
using System.Collections.Generic;

namespace BookFast.SeedWork.Modeling
{
    public abstract class Entity<TIdentity> : IEntity
    {
        private TIdentity id = default(TIdentity);
        private List<Event> events;
                
        public TIdentity Id
        {
            get { return id; }
            set
            {
                if (!id.Equals(default(TIdentity)))
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

        public virtual IEnumerable<Event> CollectEvents() => events;
    }
}
