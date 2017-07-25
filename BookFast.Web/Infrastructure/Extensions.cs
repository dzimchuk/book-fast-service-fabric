using BookFast.Web.Infrastructure.Authentication.Customer;
using System;
using System.Security.Claims;

namespace BookFast.Web.Infrastructure
{
    internal static class Extensions
    {
        public static bool IsB2CAuthenticated(this ClaimsPrincipal principal, B2CPolicies policies)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var acrClaim = principal.FindFirst(B2CAuthConstants.AcrClaimType);
                return acrClaim != null &&
                    (acrClaim.Value.Equals(policies.SignInOrSignUpPolicy, StringComparison.OrdinalIgnoreCase) ||
                     acrClaim.Value.Equals(policies.EditProfilePolicy, StringComparison.OrdinalIgnoreCase)
                    );
            }

            return false;
        }
    }
}
