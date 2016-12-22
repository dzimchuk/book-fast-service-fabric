using System.Security.Claims;

namespace BookFast.Facility.Infrastructure
{
    internal interface ISecurityContextAcceptor
    {
        ClaimsPrincipal Principal { set; }
    }
}