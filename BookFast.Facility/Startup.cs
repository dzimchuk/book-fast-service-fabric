using BookFast.Framework;
using BookFast.Security.AspNetCore;
using BookFast.ServiceFabric;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Fabric;

namespace BookFast.Facility
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly StatelessServiceContext serviceContext;

        public Startup(IConfiguration configuration, StatelessServiceContext serviceContext)
        {
            this.configuration = configuration;
            this.serviceContext = serviceContext;
        }
        
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
                module.AddServices(services, configuration);
            }

            services.AddAppInsights(configuration, serviceContext);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddAppInsights(app.ApplicationServices);
            
            app.UseAuthentication();

            app.UseSecurityContext();
            app.UseMvc();

            app.UseSwagger();
        }
    }
}
