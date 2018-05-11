using System;

namespace BookFast.SeedWork.Modeling
{
    public class IntegrationEvent : Event
    {
        public Guid EventId { get; }

        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
        }
    }
}
