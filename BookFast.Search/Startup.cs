using BookFast.SeedWork;
using BookFast.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BookFast.Search
{
    public class Startup
    {
        private const string apiTitle = "Book Fast Search API";
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
                              new Adapter.Composition.CompositionModule()
                          };

            foreach (var module in modules)
            {
                module.AddServices(services, configuration);
            }

            services.AddSwashbuckle(configuration, apiTitle, apiVersion, "BookFast.Search.xml");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddAppInsights(app.ApplicationServices);
                        
            app.UseMvc();

            app.UseSwagger(apiTitle, apiVersion);
        }
    }
}
