using System;
using System.Security.Claims;
using BookFast.Common.Security;
using BookFast.Facility.Business;

namespace BookFast.Facility.Infrastructure
{
    internal class SecurityContextProvider : ISecurityContext, ISecurityContextAcceptor
    {
        public ClaimsPrincipal Principal { get; set; }

        public string GetCurrentUser()
        {
            return FindFirstValue(BookFastClaimTypes.ObjectId);
        }

        public string GetCurrentTenant()
        {
            return FindFirstValue(BookFastClaimTypes.TenantId);
        }

        private string FindFirstValue(string claimType)
        {
            if (Principal == null)
                throw new Exception("Principal has not been initialized.");

            var claim = Principal.FindFirst(claimType);
            if (claim == null)
                throw new Exception($"Claim '{claimType}' was not found.");

            return claim.Value;
        }
    }
}