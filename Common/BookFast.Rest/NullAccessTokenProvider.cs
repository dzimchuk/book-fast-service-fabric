using System.Threading.Tasks;

namespace BookFast.Rest
{
    public class NullAccessTokenProvider : IAccessTokenProvider
    {
        public Task<string> AcquireTokenAsync() => Task.FromResult<string>(null);

        public Task<string> AcquireTokenAsync(string resource) => Task.FromResult<string>(null);
    }
}
