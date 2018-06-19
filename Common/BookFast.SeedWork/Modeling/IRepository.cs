using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.SeedWork.Modeling
{
    public interface IRepository
    {
        Task PersistEventsAsync(IEnumerable<Event> events);
        Task SaveChangesAsync();
    }

    public interface IRepository<T> : IRepository where T: IAggregateRoot
    {
    }
}
