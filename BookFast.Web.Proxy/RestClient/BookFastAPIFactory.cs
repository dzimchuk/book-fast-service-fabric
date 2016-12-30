using System;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.Extensions.Options;

namespace BookFast.Web.Proxy.RestClient
{
    internal class BookFastAPIFactory : IBookFastAPIFactory
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;
        
        public BookFastAPIFactory(IAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
        {
            this.accessTokenProvider = accessTokenProvider;
            this.apiOptions = apiOptions.Value;
        }

        public async Task<IBookFastAPI> CreateAsync()
        {
            throw new NotImplementedException();
        }
    }
}