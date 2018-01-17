using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using System;
using BookFast.ServiceFabric.Communication;
using BookFast.Rest;

namespace BookFast.Booking.Client
{
    internal class BookingCommunicationClientFactory : CommunicationClientFactoryBase<CommunicationClient<IBookFastBookingAPI>>
    {
        private readonly IApiClientFactory<IBookFastBookingAPI> apiClientFactory;

        public BookingCommunicationClientFactory(IServicePartitionResolver resolver, IApiClientFactory<IBookFastBookingAPI> apiClientFactory)
            : base(resolver, new[] { new HttpExceptionHandler() })
        {
            this.apiClientFactory = apiClientFactory;
        }

        protected override void AbortClient(CommunicationClient<IBookFastBookingAPI> client)
        {
        }

        protected override Task<CommunicationClient<IBookFastBookingAPI>> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            var client = new CommunicationClient<IBookFastBookingAPI>(() =>
            {
                return apiClientFactory.CreateApiClientAsync(new Uri(endpoint));
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
