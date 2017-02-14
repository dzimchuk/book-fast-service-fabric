using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.Rest;
using System;
using BookFast.ServiceFabric.Communication;
using BookFast.Rest;

namespace BookFast.Booking.Client
{
    internal class BookingCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastBookingAPI>>
    {
        private readonly ICustomerAccessTokenProvider accessTokenProvider;

        public BookingCommunicationClientFactory(IServicePartitionResolver resolver, ICustomerAccessTokenProvider accessTokenProvider)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            if (accessTokenProvider == null)
            {
                throw new ArgumentNullException(nameof(accessTokenProvider));
            }

            this.accessTokenProvider = accessTokenProvider;
        }

        protected override void AbortClient(CommunicationClient<IBookFastBookingAPI> client)
        {
        }

        protected override Task<CommunicationClient<IBookFastBookingAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            var client = new CommunicationClient<IBookFastBookingAPI>(async () =>
            {
                var accessToken = await accessTokenProvider.AcquireTokenAsync();
                var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

                return new BookFastBookingAPI(new Uri(endpoint), credentials);
            });

            return Task.FromResult(client);
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
