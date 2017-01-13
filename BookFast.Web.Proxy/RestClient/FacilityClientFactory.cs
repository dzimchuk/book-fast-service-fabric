using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.Extensions.Options;

namespace BookFast.Web.Proxy.RestClient
{
    internal class FacilityClientFactory : CommunicationClientFactoryBase<FacilityClient>
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;

        public FacilityClientFactory(IServicePartitionResolver resolver, IAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.accessTokenProvider = accessTokenProvider;
            this.apiOptions = apiOptions.Value;
        }

        protected override void AbortClient(FacilityClient client)
        {
            // client with persistent connections should abort their connections here.
            // HTTP clients don't hold persistent connections, so no action is taken.
        }

        protected override Task<FacilityClient> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            // clients that maintain persistent connections to a service should 
            // create that connection here.
            // an HTTP client doesn't maintain a persistent connection.
            
            return Task.FromResult(new FacilityClient(accessTokenProvider, apiOptions.FacilityServiceApiResource, endpoint));
        }

        protected override bool ValidateClient(FacilityClient client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }

        protected override bool ValidateClient(string endpoint, FacilityClient client)
        {
            // client with persistent connections should be validated here.
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return true;
        }
    }
}
