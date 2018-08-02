using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents
{
    public interface IReliableEventsDataSource
    {
        Task<IEnumerable<ReliableEvent>> GetPendingEventsAsync(CancellationToken cancellationToken);
        Task ClearEventAsync(string eventId, CancellationToken cancellationToken);
    }
}
