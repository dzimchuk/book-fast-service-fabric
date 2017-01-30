using BookFast.Web.Contracts.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Experimental.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication
{
    internal static class B2CAuthentication
    {
        public static async Task<string> AcquireAccessTokenAsync(B2CAuthenticationOptions authOptions)
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

        public static void UseOpenIdConnectB2CAuthentication(this IApplicationBuilder app, B2CAuthenticationOptions authOptions, B2CPolicies b2cPolicies, bool automaticChallenge = false)
        {
            var openIdConnectOptions = new OpenIdConnectOptions
            {
                AuthenticationScheme = AuthConstants.OpenIdConnectB2CAuthenticationScheme,
                AutomaticChallenge = automaticChallenge,

                CallbackPath = new PathString(AuthConstants.B2CCallbackPath),

                Authority = authOptions.Authority,
                ClientId = authOptions.ClientId,
                ClientSecret = authOptions.ClientSecret,
                PostLogoutRedirectUri = authOptions.PostLogoutRedirectUri,

                ConfigurationManager = new PolicyConfigurationManager(authOptions.Authority,
                                               new[] { b2cPolicies.SignInOrSignUpPolicy, b2cPolicies.EditProfilePolicy }),
                Events = CreateOpenIdConnectEventHandlers(authOptions, b2cPolicies),

                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                },

                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme
            };

            openIdConnectOptions.Scope.Add("offline_access");

            app.UseOpenIdConnectAuthentication(openIdConnectOptions);
        }

        private static IOpenIdConnectEvents CreateOpenIdConnectEventHandlers(B2CAuthenticationOptions authOptions, B2CPolicies policies)
        {
            return new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context => SetIssuerAddressAsync(context, policies.SignInOrSignUpPolicy),
                OnRedirectToIdentityProviderForSignOut = context => SetIssuerAddressForSignOutAsync(context, policies.SignInOrSignUpPolicy),
                OnAuthorizationCodeReceived = async context =>
                {
                    var credential = new ClientCredential(authOptions.ClientId, authOptions.ClientSecret);
                    var authenticationContext = new AuthenticationContext(authOptions.Authority);
                    var result = await authenticationContext.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                        new Uri(context.TokenEndpointRequest.RedirectUri, UriKind.RelativeOrAbsolute), credential,
                        new[] { authOptions.ClientId }, context.Ticket.Principal.FindFirst(AuthConstants.AcrClaimType).Value);

                    context.HandleCodeRedemption();
                },
                OnTokenValidated = context =>
                {
                    var claimsIdentity = (ClaimsIdentity)context.Ticket.Principal.Identity;
                    claimsIdentity.AddClaim(new Claim(BookFastClaimTypes.InteractorRole, InteractorRole.Customer.ToString()));
                    return Task.FromResult(0);
                },
                OnAuthenticationFailed = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/home/error");
                    return Task.FromResult(0);
                },
                OnMessageReceived = context =>
                {
                    if (!string.IsNullOrEmpty(context.ProtocolMessage.Error) &&
                        !string.IsNullOrEmpty(context.ProtocolMessage.ErrorDescription) &&
                        context.ProtocolMessage.ErrorDescription.StartsWith("AADB2C90091") &&
                        context.Properties.Items[AuthConstants.B2CPolicy] == policies.EditProfilePolicy)
                    {
                        context.Ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(context.HttpContext.User, context.Properties, AuthConstants.OpenIdConnectB2CAuthenticationScheme);
                        context.HandleResponse();
                    }

                    return Task.FromResult(0);
                }
            };
        }

        private static async Task SetIssuerAddressAsync(RedirectContext context, string defaultPolicy)
        {
            var configuration = await GetOpenIdConnectConfigurationAsync(context, defaultPolicy);
            context.ProtocolMessage.IssuerAddress = configuration.AuthorizationEndpoint;
        }

        private static async Task SetIssuerAddressForSignOutAsync(RedirectContext context, string defaultPolicy)
        {
            var configuration = await GetOpenIdConnectConfigurationAsync(context, defaultPolicy);
            context.ProtocolMessage.IssuerAddress = configuration.EndSessionEndpoint;
        }

        private static async Task<OpenIdConnectConfiguration> GetOpenIdConnectConfigurationAsync(RedirectContext context, string defaultPolicy)
        {
            var manager = (PolicyConfigurationManager)context.Options.ConfigurationManager;
            var policy = context.Properties.Items.ContainsKey(AuthConstants.B2CPolicy) ? context.Properties.Items[AuthConstants.B2CPolicy] : defaultPolicy;
            var configuration = await manager.GetConfigurationByPolicyAsync(CancellationToken.None, policy);
            return configuration;
        }
    }
}
