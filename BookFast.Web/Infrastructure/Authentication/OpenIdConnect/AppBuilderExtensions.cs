using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace BookFast.Web.Infrastructure.Authentication.OpenIdConnect
{
    internal static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseB2COpenIdConnectAuthentication(this IApplicationBuilder app, OpenIdConnectOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<B2COpenIdConnectMiddleware>(Options.Create(options));
        }
    }
}
