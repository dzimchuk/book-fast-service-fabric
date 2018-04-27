using System.Threading.Tasks;

namespace BookFast.SeedWork.Modeling
{
    public interface IRepository<T> where T: IAggregateRoot
    {
        Task PersistEventsAsync(T entity);
    }
}
