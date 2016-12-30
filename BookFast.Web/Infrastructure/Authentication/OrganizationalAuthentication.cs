using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication
{
    internal static class OrganizationalAuthentication
    {
        public static async Task<string> AcquireAccessTokenAsync(AuthenticationOptions authOptions, string userId, string resource)
        {
            var clientCredential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
            var authenticationContext = new AuthenticationContext(authOptions.Authority);

            try
            {
                var user = !string.IsNullOrEmpty(userId) ? new UserIdentifier(userId, UserIdentifierType.UniqueId) : UserIdentifier.AnyUser;
                var authenticationResult = await authenticationContext.AcquireTokenSilentAsync(resource,
                    clientCredential, user);

                return authenticationResult.AccessToken;
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                return null;
            }
        }

        public static void UseOpenIdConnectOrganizationalAuthentication(this IApplicationBuilder app, AuthenticationOptions authOptions, bool automaticChallenge = false)
        {
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = AuthConstants.OpenIdConnectOrganizationalAuthenticationScheme,
                AutomaticChallenge = automaticChallenge,

                CallbackPath = new PathString(AuthConstants.OrganizationalCallbackPath),

                Authority = authOptions.Authority,
                ClientId = authOptions.ClientId,
                ClientSecret = authOptions.ClientSecret,

                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuers = authOptions.ValidIssuersAsArray
                },

                ResponseType = OpenIdConnectResponseType.CodeIdToken,

                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                PostLogoutRedirectUri = authOptions.PostLogoutRedirectUri,

                Events = CreateOpenIdConnectEventHandlers(authOptions)
            });
        }

        private static IOpenIdConnectEvents CreateOpenIdConnectEventHandlers(AuthenticationOptions authOptions)
        {
            return new OpenIdConnectEvents
            {
                OnAuthorizationCodeReceived = async context =>
                {
                    var clientCredential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
                    var authenticationContext = new AuthenticationContext(authOptions.Authority);
                    await authenticationContext.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                        new Uri(context.TokenEndpointRequest.RedirectUri, UriKind.RelativeOrAbsolute), clientCredential, authOptions.ApiResource);

                    context.HandleCodeRedemption();
                }
            };
        }
    }
}
