using System;
using System.Threading.Tasks;
using BookFast.Rest;
using Microsoft.Extensions.Options;

namespace BookFast.Search.Client
{
    internal class SearchApiClientFactory : IApiClientFactory<IBookFastSearchAPI>
    {
        private readonly ApiOptions apiOptions;

        public SearchApiClientFactory(IOptions<ApiOptions> apiOptions)
        {
            this.apiOptions = apiOptions.Value;
        }

        public Task<IBookFastSearchAPI> CreateApiClientAsync()
        {
            return CreateApiClientAsync(new Uri(apiOptions.ServiceUri));
        }

        public Task<IBookFastSearchAPI> CreateApiClientAsync(Uri baseUri)
        {
            return Task.FromResult((IBookFastSearchAPI)new BookFastSearchAPI(baseUri));
        }
    }
}
