using System.Threading.Tasks;

namespace BookFast.Web.Proxy
{
    public interface IBookFastAPIFactory
    {
        Task<IBookFastAPI> CreateAsync();
    }
}