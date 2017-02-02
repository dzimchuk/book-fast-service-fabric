using BookFast.Rest;
using Microsoft.Experimental.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication.Customer
{
    internal class CustomerAccessTokenProvider : ICustomerAccessTokenProvider
    {
        private readonly B2CAuthenticationOptions authOptions;

        public CustomerAccessTokenProvider(IOptions<B2CAuthenticationOptions> authOptions)
        {
            this.authOptions = authOptions.Value;
        }

        public async Task<string> AcquireTokenAsync()
        {
            var credential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
            var authenticationContext = new AuthenticationContext(authOptions.Authority);
            try
            {
                var result = await authenticationContext.AcquireTokenSilentAsync(new[] { authOptions.ClientId }, credential, UserIdentifier.AnyUser);
                return result.Token;
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                return null;
            }
        }
    }
}
