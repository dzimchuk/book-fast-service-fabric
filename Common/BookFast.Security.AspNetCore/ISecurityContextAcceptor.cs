using System.Security.Claims;

namespace BookFast.Security.AspNetCore
{
    internal interface ISecurityContextAcceptor
    {
        ClaimsPrincipal Principal { set; }
    }
}