using BookFast.SeedWork.Modeling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents.CommandStack
{
    public interface IRepositoryWithReliableEvents<TEntity> : IRepository<TEntity> where TEntity : IAggregateRoot, IEntity
    {
        Task PersistEventsAsync(IEnumerable<ReliableEvent> events);
        Task SaveChangesAsync();
    }
}
