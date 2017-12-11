using BookFast.Web.Contracts.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication.Customer
{
    internal static class B2CAuthenticationExtensions
    {
        public static AuthenticationBuilder AddOpenIdConnectB2CAuthentication(this AuthenticationBuilder authenticationBuilder, 
            B2CAuthenticationOptions authOptions, B2CPolicies b2cPolicies, IDistributedCache distributedCache)
        {
            authenticationBuilder.AddOpenIdConnect(B2CAuthConstants.OpenIdConnectB2CAuthenticationScheme, options =>
            {
                options.CallbackPath = new PathString(B2CAuthConstants.B2CCallbackPath);

                options.Authority = authOptions.Authority;
                options.ClientId = authOptions.ClientId;
                options.ClientSecret = authOptions.ClientSecret;
                options.SignedOutRedirectUri = authOptions.PostLogoutRedirectUri;

                options.ConfigurationManager = new PolicyConfigurationManager(authOptions.Authority,
                                               new[] { b2cPolicies.SignInOrSignUpPolicy, b2cPolicies.EditProfilePolicy, b2cPolicies.ResetPasswordPolicy });

                options.Events = CreateOpenIdConnectEventHandlers(authOptions, b2cPolicies, distributedCache);

                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                };

                // it will fall back on using DefaultSignInScheme if not set
                //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Scope.Add("offline_access");
                options.Scope.Add($"{authOptions.ApiIdentifier}/read_booking");
                options.Scope.Add($"{authOptions.ApiIdentifier}/update_booking");
            });

            return authenticationBuilder;
        }

        private static OpenIdConnectEvents CreateOpenIdConnectEventHandlers(B2CAuthenticationOptions authOptions, 
            B2CPolicies policies, IDistributedCache distributedCache)
        {
            return new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context => SetIssuerAddressAsync(context, policies.SignInOrSignUpPolicy),
                OnRedirectToIdentityProviderForSignOut = context => SetIssuerAddressForSignOutAsync(context, policies.SignInOrSignUpPolicy),
                OnAuthorizationCodeReceived = async context =>
                {
                    try
                    {
                        var principal = context.Principal;

                        var userTokenCache = new DistributedTokenCache(distributedCache, principal.FindFirst(B2CAuthConstants.ObjectIdClaimType).Value).GetMSALCache();
                        var client = new ConfidentialClientApplication(authOptions.ClientId,
                            authOptions.GetAuthority(principal.FindFirst(B2CAuthConstants.AcrClaimType).Value),
                            "https://app", // it's not really needed
                            new ClientCredential(authOptions.ClientSecret),
                            userTokenCache,
                            null);

                        var result = await client.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                            new[] { $"{authOptions.ApiIdentifier}/read_booking", $"{authOptions.ApiIdentifier}/update_booking" });

                        context.HandleCodeRedemption(result.AccessToken, result.IdToken);
                    }
                    catch (Exception ex)
                    {
                        context.Fail(ex);
                    }
                },
                OnTokenValidated = context =>
                {
                    var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                    claimsIdentity.AddClaim(new Claim(BookFastClaimTypes.InteractorRole, InteractorRole.Customer.ToString()));
                    return Task.FromResult(0);
                },
                OnAuthenticationFailed = context =>
                {
                    context.Fail(context.Exception);
                    return Task.FromResult(0);
                },
                OnMessageReceived = context =>
                {
                    if (!string.IsNullOrEmpty(context.ProtocolMessage.Error) &&
                        !string.IsNullOrEmpty(context.ProtocolMessage.ErrorDescription))
                    {
                        if (context.ProtocolMessage.ErrorDescription.StartsWith("AADB2C90091")) // cancel profile editing
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/");
                        }
                        else if (context.ProtocolMessage.ErrorDescription.StartsWith("AADB2C90118")) // forgot password
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Account/ResetPassword");
                        }
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

        private static Task<OpenIdConnectConfiguration> GetOpenIdConnectConfigurationAsync(RedirectContext context, string defaultPolicy)
        {
            var manager = (PolicyConfigurationManager)context.Options.ConfigurationManager;
            var policy = context.Properties.Items.ContainsKey(B2CAuthConstants.B2CPolicy) ? context.Properties.Items[B2CAuthConstants.B2CPolicy] : defaultPolicy;

            return manager.GetConfigurationByPolicyAsync(CancellationToken.None, policy);
        }
    }
}
