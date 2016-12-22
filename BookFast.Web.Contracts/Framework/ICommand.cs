using System.Threading.Tasks;

namespace BookFast.Web.Contracts.Framework
{
    public interface ICommand<in TModel>
    {
        Task ApplyAsync(TModel model);
    }
}