using System.Threading.Tasks;

namespace BookFast.SeedWork
{
    public interface ICommand<in TModel>
    {
        Task ApplyAsync(TModel model);
    }
}