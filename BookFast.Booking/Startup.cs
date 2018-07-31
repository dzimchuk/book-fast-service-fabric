using BookFast.SeedWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BookFast.Booking
{
    public class Startup
    {
        private const string apiTitle = "Book Fast Booking API";
        private const string apiVersion = "v1";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var modules = new List<ICompositionModule>
                          {
                              new Composition.CompositionModule(),
                              new CommandStack.Composition.CompositionModule(),
                              new Data.Composition.CompositionModule()
                          };

            foreach (var module in modules)
            {
                module.AddServices(services, configuration);
            }

            services.AddSwashbuckle(configuration, apiTitle, apiVersion, "BookFast.Booking.xml");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddAppInsights(app.ApplicationServices);
            
            app.UseAuthentication();

            app.UseSecurityContext();
            app.UseMvc();

            app.UseSwagger(apiTitle, apiVersion);
        }
    }
}
