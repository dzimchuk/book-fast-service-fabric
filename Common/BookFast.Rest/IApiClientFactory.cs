using System;
using System.Threading.Tasks;

namespace BookFast.Rest
{
    public interface IApiClientFactory<T>
    {
        Task<T> CreateApiClientAsync();
        Task<T> CreateApiClientAsync(Uri baseUri);
    }
}
