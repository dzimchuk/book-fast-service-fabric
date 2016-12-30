using System.Threading.Tasks;

namespace BookFast.Web.Proxy.RestClient
{
    public interface IAccessTokenProvider
    {
        Task<string> AcquireTokenAsync(string resource);
    }
}
