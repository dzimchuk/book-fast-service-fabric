using BookFast.ReliableEvents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task ClearEventAsync(int eventId, CancellationToken cancellationToken)
        {
            var trackedEvent = await context.Events.FindAsync(eventId);
            context.Events.Remove(trackedEvent);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReliableEvent>> GetPendingEventsAsync(CancellationToken cancellationToken)
        {
            var events = await context.Events.ToListAsync(cancellationToken);
            return events;
        }
    }
}
