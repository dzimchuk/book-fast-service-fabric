using BookFast.Booking.Data.Mappers;
using BookFast.ReliableEvents;
using Microsoft.ServiceFabric.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Booking.Data
{
    internal class ReliableEventsDataSource : IReliableEventsDataSource
    {
        private readonly IReliableStateManager stateManager;

        public ReliableEventsDataSource(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task ClearEventAsync(string eventId, CancellationToken cancellationToken)
        {
            var eventsDictionary = await stateManager.GetAllReliableEventsAsync();

            using (var transaction = stateManager.CreateTransaction())
            {
                await eventsDictionary.TryRemoveAsync(transaction, eventId);

                await transaction.CommitAsync();
            }
        }

        public async Task<IEnumerable<ReliableEvent>> GetPendingEventsAsync(CancellationToken cancellationToken)
        {
            var eventsDictionary = await stateManager.GetAllReliableEventsAsync();
            var result = new List<ReliableEvent>();

            using (var transaction = stateManager.CreateTransaction())
            {
                var enumerator = (await eventsDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    result.Add(enumerator.Current.Value.ToContractModel());
                }

                await transaction.CommitAsync();
            }

            return result;
        }
    }
}
