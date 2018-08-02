using BookFast.Facility.Data.Mappers;
using BookFast.ReliableEvents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Facility.Data
{
    internal class ReliableEventsDataSource : IReliableEventsDataSource
    {
        private readonly FacilityContext context;

        public ReliableEventsDataSource(FacilityContext context)
        {
            this.context = context;
        }

        public async Task ClearEventAsync(string eventId, CancellationToken cancellationToken)
        {
            var trackedEvent = await context.Events.FindAsync(int.Parse(eventId));
            context.Events.Remove(trackedEvent);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReliableEvent>> GetPendingEventsAsync(CancellationToken cancellationToken)
        {
            var events = await context.Events.ToListAsync(cancellationToken);
            return events.Select(@event => @event.ToContractModel());
        }
    }
}
