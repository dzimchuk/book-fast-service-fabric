using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication
{
    internal class CustomChallengeResult : ChallengeResult
    {
        private readonly ChallengeBehavior behavior;

        public CustomChallengeResult(string authenticationScheme, AuthenticationProperties properties, ChallengeBehavior behavior)
            : base(authenticationScheme, properties)
        {
            this.behavior = behavior;
        }
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<CustomChallengeResult>();

            var authentication = context.HttpContext.Authentication;

            if (AuthenticationSchemes != null && AuthenticationSchemes.Count > 0)
            {
                logger.LogInformation("Executing CustomChallengeResult with authentication schemes: {0}.", AuthenticationSchemes.Aggregate((aggr, current) => $"{aggr}, {current}"));

                foreach (var scheme in AuthenticationSchemes)
                {
                    await authentication.ChallengeAsync(scheme, Properties, behavior);
                }
            }
            else
            {
                logger.LogInformation("Executing CustomChallengeResult.");
                await authentication.ChallengeAsync(Properties);
            }
        }
    }
}
