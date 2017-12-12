using BookFast.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication.Organizational
{
    internal class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly AuthenticationOptions authOptions;
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDistributedCache distributedCache;

        public AccessTokenProvider(IOptions<AuthenticationOptions> authOptions, 
            IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, 
            IDistributedCache distributedCache)
        {
            this.authOptions = authOptions.Value;
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
            this.distributedCache = distributedCache;
        }

        public async Task<string> AcquireTokenAsync(string resource)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(httpContextAccessor.HttpContext?.User, "FacilityProviderOnly");
            if (!authorizationResult.Succeeded)
            {
                return null;
            }

            var userId = GetUserId();

            var clientCredential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
            var authenticationContext = new AuthenticationContext(authOptions.Authority, new DistributedTokenCache(distributedCache, userId));

            try
            {
                var user = !string.IsNullOrEmpty(userId) ? new UserIdentifier(userId, UserIdentifierType.UniqueId) : UserIdentifier.AnyUser;
                var authenticationResult = await authenticationContext.AcquireTokenSilentAsync(resource,
                    clientCredential, user);

                return authenticationResult.AccessToken;
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

            return httpContextAccessor.HttpContext.User.FindFirst(AuthConstants.ObjectIdClaimType)?.Value;
        }
    }
}
