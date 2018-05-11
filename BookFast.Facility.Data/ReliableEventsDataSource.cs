using BookFast.ReliableEvents;
using BookFast.SeedWork.Modeling;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.Data
{
    internal class ReliableEventsDataSource : IReliableEventsDataSource
    {
        private readonly FacilityContext context;
        private readonly Dictionary<string, Type> eventTypes = new Dictionary<string, Type>
        {
            { "FacilityCreatedEvent", typeof(Domain.Events.FacilityCreatedEvent) },
            { "FacilityUpdatedEvent", typeof(Domain.Events.FacilityUpdatedEvent) },
            { "FacilityDeletedEvent", typeof(Domain.Events.FacilityDeletedEvent) },
            { "AccommodationCreatedEvent", typeof(Domain.Events.AccommodationCreatedEvent) },
            { "AccommodationUpdatedEvent", typeof(Domain.Events.AccommodationUpdatedEvent) },
            { "AccommodationDeletedEvent", typeof(Domain.Events.AccommodationDeletedEvent) },
        };

        public ReliableEventsDataSource(FacilityContext context)
        {
            this.context = context;
        }

        public async Task ClearEventAsync(int eventId, CancellationToken cancellationToken)
        {
            var trackedEvent = await context.Events.FindAsync(eventId);
            context.Events.Remove(trackedEvent);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReliableEvent>> GetPendingEventsAsync(CancellationToken cancellationToken)
        {
            var events = await context.Events.ToListAsync(cancellationToken);
            return (from @event in events
                    let type = eventTypes[@event.EventName]
                    select new ReliableEvent
                    {
                        Id = @event.Id,
                        User = @event.User,
                        Tenant = @event.Tenant,
                        Event = (Event)JsonConvert.DeserializeObject(@event.Payload, type)
                    }).ToArray();
        }
    }
}
