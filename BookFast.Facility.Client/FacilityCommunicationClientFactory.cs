using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using System;
using BookFast.ServiceFabric.Communication;
using BookFast.Rest;

namespace BookFast.Facility.Client
{
    internal class FacilityCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastFacilityAPI>>
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;

        public FacilityCommunicationClientFactory(IServicePartitionResolver resolver, IAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            if (accessTokenProvider == null)
            {
                throw new ArgumentNullException(nameof(accessTokenProvider));
            }

            this.accessTokenProvider = accessTokenProvider;
            this.apiOptions = apiOptions.Value;
        }

        protected override void AbortClient(CommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should abort their connections here.
            // HTTP clients don't hold persistent connections, so no action is taken.
        }

        protected override Task<CommunicationClient<IBookFastFacilityAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            // clients that maintain persistent connections to a service should 
            // create that connection here.
            // an HTTP client doesn't maintain a persistent connection.

            var client = new CommunicationClient<IBookFastFacilityAPI>(async () =>
            {
                var accessToken = await accessTokenProvider.AcquireTokenAsync(apiOptions.ServiceApiResource);
                var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

                return new BookFastFacilityAPI(new Uri(endpoint), credentials);
            });

            return Task.FromResult(client);
        }

        protected override bool ValidateClient(CommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }

        protected override bool ValidateClient(string endpoint, CommunicationClient<IBookFastFacilityAPI> client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }
    }
}
