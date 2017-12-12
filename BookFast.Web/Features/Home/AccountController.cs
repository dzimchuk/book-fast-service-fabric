using BookFast.Web.Infrastructure;
using BookFast.Web.Infrastructure.Authentication.Customer;
using BookFast.Web.Infrastructure.Authentication.Organizational;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                        B2CAuthConstants.OpenIdConnectB2CAuthenticationScheme, 
                        new AuthenticationProperties(new Dictionary<string, string> { { B2CAuthConstants.B2CPolicy, policies.SignInOrSignUpPolicy } })
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
                return new ChallengeResult(
                    B2CAuthConstants.OpenIdConnectB2CAuthenticationScheme,
                    new AuthenticationProperties(new Dictionary<string, string> { { B2CAuthConstants.B2CPolicy, policies.EditProfilePolicy } })
                    {
                        RedirectUri = "/"
                    });
            }

            return RedirectHome();
        }

        public IActionResult ResetPassword()
        {
            return new ChallengeResult(
                    B2CAuthConstants.OpenIdConnectB2CAuthenticationScheme,
                    new AuthenticationProperties(new Dictionary<string, string> { { B2CAuthConstants.B2CPolicy, policies.ResetPasswordPolicy } })
                    {
                        RedirectUri = "/"
                    });
        }

        public async Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (IsB2CAuthenticated)
                {
                    await HttpContext.SignOutAsync(B2CAuthConstants.OpenIdConnectB2CAuthenticationScheme,
                                new AuthenticationProperties(new Dictionary<string, string> { { B2CAuthConstants.B2CPolicy, User.FindFirst(B2CAuthConstants.AcrClaimType).Value } })); 
                }
                else
                {
                    await HttpContext.SignOutAsync(AuthConstants.OpenIdConnectOrganizationalAuthenticationScheme);
                }

                return new EmptyResult(); 
            }

            return RedirectHome();
        }

        private bool IsB2CAuthenticated => User.IsB2CAuthenticated(policies);

        private IActionResult RedirectHome() => RedirectToAction(nameof(HomeController.Index), "Home");
    }
}