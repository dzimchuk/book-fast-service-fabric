using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using System;
using BookFast.ServiceFabric.Communication;
using BookFast.Rest;

namespace BookFast.Booking.Client
{
    internal class BookingCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastBookingAPI>>
    {
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;

        public BookingCommunicationClientFactory(IServicePartitionResolver resolver, IAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.accessTokenProvider = accessTokenProvider;
            this.apiOptions = apiOptions.Value;
        }

        protected override void AbortClient(CommunicationClient<IBookFastBookingAPI> client)
        {
        }

        protected override async Task<CommunicationClient<IBookFastBookingAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            var accessToken = await accessTokenProvider.AcquireTokenAsync(apiOptions.ServiceApiResource);
            var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

            return new CommunicationClient<IBookFastBookingAPI>(new BookFastBookingAPI(new Uri(endpoint), credentials));
        }

        protected override bool ValidateClient(CommunicationClient<IBookFastBookingAPI> client)
        {
            return true;
        }

        protected override bool ValidateClient(string endpoint, CommunicationClient<IBookFastBookingAPI> client)
        {
            return true;
        }
    }
}
