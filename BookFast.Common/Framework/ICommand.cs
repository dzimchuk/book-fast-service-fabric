using System.Threading.Tasks;

namespace BookFast.Common.Framework
{
    public interface ICommand<in TModel>
    {
        Task ApplyAsync(TModel model);
    }
}