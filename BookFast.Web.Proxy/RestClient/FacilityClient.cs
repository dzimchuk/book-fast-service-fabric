using Microsoft.Rest;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Fabric;
using System.Threading.Tasks;

namespace BookFast.Web.Proxy.RestClient
{
    internal class FacilityClient : ICommunicationClient
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly string apiResource;
        private readonly string endpoint;

        public FacilityClient(IAccessTokenProvider accessTokenProvider, string apiResource, string endpoint)
        {
            this.accessTokenProvider = accessTokenProvider;
            this.apiResource = apiResource;
            this.endpoint = endpoint;
        }
        
        public async Task<IBookFastFacilityAPI> GetApiAsync()
        {
            var accessToken = await accessTokenProvider.AcquireTokenAsync(apiResource);
            var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

            return new BookFastFacilityAPI(new Uri(endpoint), credentials);
        }

        ResolvedServiceEndpoint ICommunicationClient.Endpoint { get; set; }
        string ICommunicationClient.ListenerName { get; set; }
        ResolvedServicePartition ICommunicationClient.ResolvedServicePartition { get; set; }
    }
}
