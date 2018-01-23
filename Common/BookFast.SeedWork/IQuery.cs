using System.Threading.Tasks;

namespace BookFast.SeedWork
{
    public interface IQuery<in TModel, TResult>
    {
        Task<TResult> ExecuteAsync(TModel model);
    }
}