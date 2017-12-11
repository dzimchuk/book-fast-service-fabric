using BookFast.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication.Customer
{
    internal class CustomerAccessTokenProvider : ICustomerAccessTokenProvider
    {
        private readonly B2CAuthenticationOptions authOptions;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDistributedCache distributedCache;

        public CustomerAccessTokenProvider(IOptions<B2CAuthenticationOptions> authOptions, IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache)
        {
            this.authOptions = authOptions.Value;
            this.httpContextAccessor = httpContextAccessor;
            this.distributedCache = distributedCache;
        }

        public async Task<string> AcquireTokenAsync()
        {
            try
            {
                var principal = httpContextAccessor.HttpContext.User;

                var tokenCache = new DistributedTokenCache(distributedCache, principal.FindFirst(B2CAuthConstants.ObjectIdClaimType).Value).GetMSALCache();
                var client = new ConfidentialClientApplication(authOptions.ClientId,
                                                          authOptions.GetAuthority(principal.FindFirst(B2CAuthConstants.AcrClaimType).Value),
                                                          "https://app", // it's not really needed
                                                          new ClientCredential(authOptions.ClientSecret),
                                                          tokenCache,
                                                          null);

                var result = await client.AcquireTokenSilentAsync(new[] { $"{authOptions.ApiIdentifier}/read_values", $"{authOptions.ApiIdentifier}/update_booking" },
                    client.Users.FirstOrDefault());

                return result.AccessToken;
            }
            catch (MsalUiRequiredException)
            {
                throw new ReauthenticationRequiredException();
            }
        }
    }
}
