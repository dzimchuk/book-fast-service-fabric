using BookFast.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.Experimental.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
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

            var credential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
            var authenticationContext = new AuthenticationContext(authOptions.Authority, new DistributedTokenCache(distributedCache, GetUserId()));

            try
            {
                //var userIdentifier = new UserIdentifier(GetUserId(), UserIdentifierType.UniqueId);

                // AcquireTokenByAuthorizationCodeAsync of the experimental ADAL does not have an overload that accepts a user id
                // so UniqueId in the token cache key will be null and AcquireTokenSilentAsync won't be able to find anything
                // it basically means that the experimental ADAL is... yes experimental
                var userIdentifier = UserIdentifier.AnyUser;

                var result = await authenticationContext.AcquireTokenSilentAsync(new[] { authOptions.ClientId }, credential, userIdentifier);
                return result.Token;
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                throw new ReauthenticationRequiredException();
            }
        }

        private string GetUserId()
        {
            if (httpContextAccessor.HttpContext?.User == null)
                return null;

            return httpContextAccessor.HttpContext.User.FindFirst(B2CAuthConstants.ObjectId)?.Value;
        }
    }
}
