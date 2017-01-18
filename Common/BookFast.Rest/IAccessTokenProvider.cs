using System.Threading.Tasks;

namespace BookFast.Rest
{
    public interface IAccessTokenProvider
    {
        Task<string> AcquireTokenAsync(string resource);
    }
}
