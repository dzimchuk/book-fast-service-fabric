using BookFast.ReliableEvents;
using System;
using System.Collections.Generic;

namespace BookFast.Facility.Integration
{
    internal class ReliableEventMapper : IReliableEventMapper
    {
        private readonly Dictionary<string, Type> eventTypes = new Dictionary<string, Type>
        {
            { "FacilityCreatedEvent", typeof(Domain.Events.FacilityCreatedEvent) },
            { "FacilityUpdatedEvent", typeof(Domain.Events.FacilityUpdatedEvent) },
            { "FacilityDeletedEvent", typeof(Domain.Events.FacilityDeletedEvent) },
            { "AccommodationCreatedEvent", typeof(Domain.Events.AccommodationCreatedEvent) },
            { "AccommodationUpdatedEvent", typeof(Domain.Events.AccommodationUpdatedEvent) },
            { "AccommodationDeletedEvent", typeof(Domain.Events.AccommodationDeletedEvent) },
        };

        public Type GetEventType(string eventType) => eventTypes.ContainsKey(eventType) ? eventTypes[eventType] : null;
    }
}
