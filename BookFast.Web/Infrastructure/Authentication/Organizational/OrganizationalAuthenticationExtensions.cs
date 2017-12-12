using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;

namespace BookFast.Web.Infrastructure.Authentication.Organizational
{
    internal static class OrganizationalAuthenticationExtensions
    {
        public static AuthenticationBuilder AddOpenIdConnectOrganizationalAuthentication(this AuthenticationBuilder authenticationBuilder, 
            AuthenticationOptions authOptions, IDistributedCache distributedCache)
        {
            authenticationBuilder.AddOpenIdConnect(AuthConstants.OpenIdConnectOrganizationalAuthenticationScheme, options =>
            {
                options.CallbackPath = new PathString(AuthConstants.OrganizationalCallbackPath);

                options.Authority = authOptions.Authority;
                options.ClientId = authOptions.ClientId;
                options.ClientSecret = authOptions.ClientSecret;
                options.SignedOutRedirectUri = authOptions.PostLogoutRedirectUri;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuers = authOptions.ValidIssuersAsArray
                };

                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                options.Events = CreateOpenIdConnectEventHandlers(authOptions, distributedCache);
            });

            return authenticationBuilder;
        }

        private static OpenIdConnectEvents CreateOpenIdConnectEventHandlers(AuthenticationOptions authOptions,
            IDistributedCache distributedCache)
        {
            return new OpenIdConnectEvents
            {
                OnAuthorizationCodeReceived = async context =>
                {
                    var userId = context.Principal.FindFirst(AuthConstants.ObjectIdClaimType).Value;

                    var clientCredential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
                    var authenticationContext = new AuthenticationContext(authOptions.Authority, new DistributedTokenCache(distributedCache, userId));
                    var result = await authenticationContext.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                        new Uri(context.TokenEndpointRequest.RedirectUri, UriKind.RelativeOrAbsolute), clientCredential, authOptions.ApiResource);

                    context.HandleCodeRedemption(result.AccessToken, result.IdToken);
                }
            };
        }
    }
}
