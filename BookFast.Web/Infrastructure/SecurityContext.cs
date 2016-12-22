using System.Security.Claims;

namespace BookFast.Web.Infrastructure
{
    internal class SecurityContext
    {
        public ClaimsPrincipal Principal { get; set; }
    }
}
