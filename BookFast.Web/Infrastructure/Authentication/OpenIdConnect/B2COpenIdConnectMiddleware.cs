using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace BookFast.Web.Infrastructure.Authentication.OpenIdConnect
{
    internal class B2COpenIdConnectMiddleware : OpenIdConnectMiddleware
    {
        public B2COpenIdConnectMiddleware(RequestDelegate next, 
            IDataProtectionProvider dataProtectionProvider, 
            ILoggerFactory loggerFactory, 
            UrlEncoder encoder, 
            IServiceProvider services, 
            IOptions<SharedAuthenticationOptions> sharedOptions, 
            IOptions<OpenIdConnectOptions> options, 
            HtmlEncoder htmlEncoder)
            : base(next, dataProtectionProvider, loggerFactory, encoder, services, sharedOptions, options, htmlEncoder)
        {
        }

        protected override AuthenticationHandler<OpenIdConnectOptions> CreateHandler() => new B2COpenIdConnectHandler(Backchannel, HtmlEncoder);
    }
}
