using System.Threading.Tasks;

namespace BookFast.Web.Contracts.Framework
{
    public interface IQuery<in TModel, TResult>
    {
        Task<TResult> ExecuteAsync(TModel model);
    }
}