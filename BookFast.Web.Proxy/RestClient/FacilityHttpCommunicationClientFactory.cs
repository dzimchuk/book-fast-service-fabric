using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.Rest;
using Microsoft.Extensions.Options;

namespace BookFast.Web.Proxy.RestClient
{
    internal class FacilityHttpCommunicationClientFactory : CommunicationClientFactoryBase<HttpCommunicationClient<IBookFastFacilityAPI>>
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;

        public FacilityHttpCommunicationClientFactory(IServicePartitionResolver resolver, IAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.accessTokenProvider = accessTokenProvider;
            this.apiOptions = apiOptions.Value;
        }

        protected override void AbortClient(HttpCommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should abort their connections here.
            // HTTP clients don't hold persistent connections, so no action is taken.
        }

        protected override async Task<HttpCommunicationClient<IBookFastFacilityAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            // clients that maintain persistent connections to a service should 
            // create that connection here.
            // an HTTP client doesn't maintain a persistent connection.

            var accessToken = await accessTokenProvider.AcquireTokenAsync(apiOptions.FacilityServiceApiResource);
            var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

            var api = new BookFastFacilityAPI(new Uri(endpoint), credentials);
            return new HttpCommunicationClient<IBookFastFacilityAPI>(api);
        }

        protected override bool ValidateClient(HttpCommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }

        protected override bool ValidateClient(string endpoint, HttpCommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }
    }
}
