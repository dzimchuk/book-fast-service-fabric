using BookFast.Web.Infrastructure.Authentication.Customer;
using BookFast.Web.Infrastructure.Authentication.Organizational;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace BookFast.Web.Infrastructure.Authentication
{
    internal class ReauthenticationRequiredFilter : IExceptionFilter
    {
        private readonly B2CPolicies policies;

        public ReauthenticationRequiredFilter(IOptions<B2CPolicies> policies)
        {
            this.policies = policies.Value;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled && IsReauthenticationRequired(context.Exception))
            {
                if (context.HttpContext.User.IsB2CAuthenticated(policies))
                {
                    context.Result = new ChallengeResult(
                        B2CAuthConstants.OpenIdConnectB2CAuthenticationScheme,
                        new AuthenticationProperties(new Dictionary<string, string> { { B2CAuthConstants.B2CPolicy, policies.SignInOrSignUpPolicy } })
                        {
                            RedirectUri = context.HttpContext.Request.Path
                        });
                }
                else
                {
                    context.Result = new ChallengeResult(
                        AuthConstants.OpenIdConnectOrganizationalAuthenticationScheme,
                        new AuthenticationProperties
                        {
                            RedirectUri = context.HttpContext.Request.Path
                        });
                }

                context.ExceptionHandled = true;
            }
        }

        private static bool IsReauthenticationRequired(Exception exception)
        {
            if (exception is ReauthenticationRequiredException)
            {
                return true;
            }

            if (exception.InnerException != null)
            {
                return IsReauthenticationRequired(exception.InnerException);
            }

            return false;
        }
    }
}
