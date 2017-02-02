using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;

namespace BookFast.Web.Infrastructure.Authentication.Organizational
{
    internal static class OrganizationalAuthenticationExtensions
    {
        public static void UseOpenIdConnectOrganizationalAuthentication(this IApplicationBuilder app, 
            AuthenticationOptions authOptions, IDistributedCache distributedCache, bool automaticChallenge = false)
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

                Events = CreateOpenIdConnectEventHandlers(authOptions, distributedCache)
            });
        }

        private static IOpenIdConnectEvents CreateOpenIdConnectEventHandlers(AuthenticationOptions authOptions,
            IDistributedCache distributedCache)
        {
            return new OpenIdConnectEvents
            {
                OnAuthorizationCodeReceived = async context =>
                {
                    var userId = context.Ticket.Principal.FindFirst(AuthConstants.ObjectId).Value;

                    var clientCredential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
                    var authenticationContext = new AuthenticationContext(authOptions.Authority, new DistributedTokenCache(distributedCache, userId));
                    await authenticationContext.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                        new Uri(context.TokenEndpointRequest.RedirectUri, UriKind.RelativeOrAbsolute), clientCredential, authOptions.ApiResource);

                    context.HandleCodeRedemption();
                }
            };
        }
    }
}
