using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace BookFast.Web.Infrastructure.Authentication.OpenIdConnect
{
    internal class B2COpenIdConnectHandler : OpenIdConnectHandler
    {
        public B2COpenIdConnectHandler(HttpClient backchannel, HtmlEncoder htmlEncoder) : base(backchannel, htmlEncoder)
        {
        }

        protected override Task<bool> HandleForbiddenAsync(ChallengeContext context) => base.HandleUnauthorizedAsync(context);
    }
}
