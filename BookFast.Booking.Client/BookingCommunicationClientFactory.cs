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

        public BookingCommunicationClientFactory(IServicePartitionResolver resolver, IAccessTokenProvider accessTokenProvider)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.accessTokenProvider = accessTokenProvider;
        }

        protected override void AbortClient(CommunicationClient<IBookFastBookingAPI> client)
        {
        }

        protected override async Task<CommunicationClient<IBookFastBookingAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            var accessToken = await accessTokenProvider.AcquireTokenAsync();
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
