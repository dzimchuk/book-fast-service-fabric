using System;
using System.Threading.Tasks;
using BookFast.Rest;
using Microsoft.Extensions.Options;
using Microsoft.Rest;

namespace BookFast.Facility.Client
{
    internal class FacilityApiClientFactory : IApiClientFactory<IBookFastFacilityAPI>
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;

        public FacilityApiClientFactory(IAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
        {
            this.accessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
            this.apiOptions = apiOptions.Value;
        }

        public Task<IBookFastFacilityAPI> CreateApiClientAsync()
        {
            return CreateApiClientAsync(new Uri(apiOptions.ServiceUri));
        }

        public async Task<IBookFastFacilityAPI> CreateApiClientAsync(Uri baseUri)
        {
            var accessToken = await accessTokenProvider.AcquireTokenAsync(apiOptions.ServiceApiResource);
            var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

            return new BookFastFacilityAPI(baseUri, credentials);
        }
    }
}
