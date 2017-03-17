using BookFast.Framework;
using BookFast.Web.Infrastructure.Authentication.Customer;
using BookFast.Web.Infrastructure.Authentication.Organizational;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Fabric;

namespace BookFast.Web
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
                              new Proxy.Composition.CompositionModule()
                          };

            foreach (var module in modules)
            {
                module.AddServices(services, Configuration);
            }

            services.AddApplicationInsightsTelemetry(Configuration);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IOptions<Infrastructure.Authentication.Organizational.AuthenticationOptions> authOptions, 
            IOptions<B2CAuthenticationOptions> b2cAuthOptions, IOptions<B2CPolicies> b2cPolicies,
            IDistributedCache distributedCache)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true
            });

            app.UseOpenIdConnectB2CAuthentication(b2cAuthOptions.Value, b2cPolicies.Value, distributedCache, true);
            app.UseOpenIdConnectOrganizationalAuthentication(authOptions.Value, distributedCache, false);
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
