using System.Security.Claims;

namespace BookFast.Security
{
    public interface ISecurityContextAcceptor
    {
        ClaimsPrincipal Principal { set; }
    }
}