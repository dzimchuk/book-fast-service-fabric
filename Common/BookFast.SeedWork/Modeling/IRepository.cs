using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.SeedWork.Modeling
{
    public interface IRepository<TEntity> where TEntity: IAggregateRoot, IEntity
    {
        Task PersistEventsAsync(IEnumerable<Event> events);
        Task SaveChangesAsync();
    }
}
