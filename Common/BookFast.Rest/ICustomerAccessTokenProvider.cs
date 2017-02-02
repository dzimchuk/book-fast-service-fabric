using System.Threading.Tasks;

namespace BookFast.Rest
{
    public interface ICustomerAccessTokenProvider
    {
        Task<string> AcquireTokenAsync();
    }
}
