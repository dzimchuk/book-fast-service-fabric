using System.Threading.Tasks;

namespace BookFast.Framework
{
    public interface ICommand<in TModel>
    {
        Task ApplyAsync(TModel model);
    }
}