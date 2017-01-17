using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Fabric;
using Microsoft.Extensions.Configuration;
using BookFast.Framework;
using Microsoft.Extensions.Options;
using BookFast.Security.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Threading.Tasks;
using BookFast.Security.AspNetCore;

namespace BookFast.Booking
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, StatelessServiceContext serviceContext)
        {
            var builder = new ConfigurationBuilder()
                .AddServiceFabricConfiguration(serviceContext);

            if (env.IsDevelopment())
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var modules = new List<ICompositionModule>
                          {
                              new Composition.CompositionModule(),
                              new Security.AspNetCore.Composition.CompositionModule(),
                              new Business.Composition.CompositionModule(),
                              new Data.Composition.CompositionModule()
                          };

            foreach (var module in modules)
            {
                module.AddServices(services, Configuration);
            }

            services.AddApplicationInsightsTelemetry(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IOptions<B2CAuthenticationOptions> b2cAuthOptions)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AuthenticationScheme = Constants.CustomerAuthenticationScheme,
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,

                MetadataAddress = $"{b2cAuthOptions.Value.Authority}/.well-known/openid-configuration?p={b2cAuthOptions.Value.Policy}",
                Audience = b2cAuthOptions.Value.Audience,

                Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        var nameClaim = ctx.Ticket.Principal.FindFirst("name");
                        if (nameClaim != null)
                        {
                            var claimsIdentity = (ClaimsIdentity)ctx.Ticket.Principal.Identity;
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, nameClaim.Value));
                        }
                        return Task.FromResult(0);
                    },
                    OnAuthenticationFailed = ctx =>
                    {
                        ctx.SkipToNextMiddleware();
                        return Task.FromResult(0);
                    }
                }
            });

            app.UseSecurityContext();
            app.UseMvc();

            app.UseSwagger();
        }
    }
}
