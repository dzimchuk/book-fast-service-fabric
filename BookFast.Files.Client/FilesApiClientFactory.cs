using System;
using System.Threading.Tasks;
using BookFast.Rest;
using Microsoft.Extensions.Options;
using Microsoft.Rest;

namespace BookFast.Files.Client
{
    internal class FilesApiClientFactory : IApiClientFactory<IBookFastFilesAPI>
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;

        public FilesApiClientFactory(IAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
        {
            this.accessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
            this.apiOptions = apiOptions.Value;
        }

        public Task<IBookFastFilesAPI> CreateApiClientAsync()
        {
            return CreateApiClientAsync(new Uri(apiOptions.ServiceUri));
        }

        public async Task<IBookFastFilesAPI> CreateApiClientAsync(Uri baseUri)
        {
            var accessToken = await accessTokenProvider.AcquireTokenAsync(apiOptions.ServiceApiResource);
            var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

            return new BookFastFilesAPI(baseUri, credentials);
        }
    }
}
