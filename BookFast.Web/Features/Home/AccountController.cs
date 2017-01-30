using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System;
using BookFast.Web.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace BookFast.Web.Features.Home
{
    public class AccountController : Controller
    {
        private readonly B2CPolicies policies;

        public AccountController(IOptions<B2CPolicies> policies)
        {
            this.policies = policies.Value;
        }

        public IActionResult SignInInternal()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(
                        AuthConstants.OpenIdConnectOrganizationalAuthenticationScheme,
                        new AuthenticationProperties
                        {
                            RedirectUri = "/"
                        });
            }

            return RedirectHome();
        }

        public IActionResult SignIn()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(
                        AuthConstants.OpenIdConnectB2CAuthenticationScheme, 
                        new AuthenticationProperties(new Dictionary<string, string> { { AuthConstants.B2CPolicy, policies.SignInOrSignUpPolicy } })
                        {
                            RedirectUri = "/"
                        });
            }

            return RedirectHome();
        }
        
        public IActionResult Profile()
        {
            if (IsB2CAuthenticated)
            {
                return new CustomChallengeResult(
                    AuthConstants.OpenIdConnectB2CAuthenticationScheme,
                    new AuthenticationProperties(new Dictionary<string, string> { { AuthConstants.B2CPolicy, policies.EditProfilePolicy } })
                    {
                        RedirectUri = "/"
                    }, ChallengeBehavior.Unauthorized);
            }

            return RedirectHome();
        }

        public async Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (IsB2CAuthenticated)
                {
                    await HttpContext.Authentication.SignOutAsync(AuthConstants.OpenIdConnectB2CAuthenticationScheme,
                                new AuthenticationProperties(new Dictionary<string, string> { { AuthConstants.B2CPolicy, User.FindFirst(AuthConstants.AcrClaimType).Value } })); 
                }
                else
                {
                    await HttpContext.Authentication.SignOutAsync(AuthConstants.OpenIdConnectOrganizationalAuthenticationScheme);
                }

                return new EmptyResult(); 
            }

            return RedirectHome();
        }

        private bool IsB2CAuthenticated
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var acrClaim = User.FindFirst(AuthConstants.AcrClaimType);
                    return acrClaim != null &&
                        (acrClaim.Value.Equals(policies.SignInOrSignUpPolicy, StringComparison.OrdinalIgnoreCase) ||
                         acrClaim.Value.Equals(policies.EditProfilePolicy, StringComparison.OrdinalIgnoreCase)
                        );
                }

                return false;
            }
        }

        private IActionResult RedirectHome() => RedirectToAction(nameof(HomeController.Index), "Home");
    }
}