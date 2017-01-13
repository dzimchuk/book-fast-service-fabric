using BookFast.Web.Infrastructure.Authentication;
using BookFast.Web.Proxy.RestClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure
{
    internal class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly AuthenticationOptions authOptions;
        private readonly B2CAuthenticationOptions b2cAuthOptions;
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        private const string ObjectId = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public AccessTokenProvider(IOptions<AuthenticationOptions> authOptions, IOptions<B2CAuthenticationOptions> b2cAuthOptions, 
            IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            this.authOptions = authOptions.Value;
            this.b2cAuthOptions = b2cAuthOptions.Value;
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> AcquireTokenAsync(string resource)
        {
            return await authorizationService.AuthorizeAsync(httpContextAccessor.HttpContext?.User, "FacilityProviderOnly") ?
                await OrganizationalAuthentication.AcquireAccessTokenAsync(authOptions, GetUserId(), resource) : await B2CAuthentication.AcquireAccessTokenAsync(b2cAuthOptions);
        }

        private string GetUserId()
        {
            if (httpContextAccessor.HttpContext?.User == null)
                return null;

            return httpContextAccessor.HttpContext.User.FindFirst(ObjectId)?.Value;
        }
    }
}
