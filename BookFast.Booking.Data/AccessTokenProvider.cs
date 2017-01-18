using BookFast.Rest;
using System.Threading.Tasks;

namespace BookFast.Booking.Data
{
    internal class AccessTokenProvider : IAccessTokenProvider
    {
        public Task<string> AcquireTokenAsync() => Task.FromResult<string>(null);

        public Task<string> AcquireTokenAsync(string resource) => Task.FromResult<string>(null);
    }
}
