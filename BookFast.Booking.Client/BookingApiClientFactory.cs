using System;
using System.Threading.Tasks;
using BookFast.Rest;
using Microsoft.Extensions.Options;
using Microsoft.Rest;

namespace BookFast.Booking.Client
{
    internal class BookingApiClientFactory : IApiClientFactory<IBookFastBookingAPI>
    {
        private readonly ICustomerAccessTokenProvider accessTokenProvider;
        private readonly ApiOptions apiOptions;

        public BookingApiClientFactory(ICustomerAccessTokenProvider accessTokenProvider, IOptions<ApiOptions> apiOptions)
        {
            this.accessTokenProvider = accessTokenProvider;
            this.apiOptions = apiOptions.Value;
        }

        public Task<IBookFastBookingAPI> CreateApiClientAsync()
        {
            return CreateApiClientAsync(new Uri(apiOptions.ServiceUri));
        }

        public async Task<IBookFastBookingAPI> CreateApiClientAsync(Uri baseUri)
        {
            var accessToken = await accessTokenProvider.AcquireTokenAsync();
            var credentials = string.IsNullOrEmpty(accessToken) ? (ServiceClientCredentials)new EmptyCredentials() : new TokenCredentials(accessToken);

            return new BookFastBookingAPI(baseUri, credentials);
        }
    }
}
