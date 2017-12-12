using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace BookFast.Swagger
{
    public static class SwaggerExtensions
    {
        public static void AddSwashbuckle(this IServiceCollection services, string title, string version, string xmlDocFileName)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(version, new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = title,
                    Version = version
                });

                options.OperationFilter<DefaultContentTypeOperationFilter>();

                options.DescribeAllEnumsAsStrings();
            });

            if (!string.IsNullOrWhiteSpace(xmlDocFileName))
            {
                AddXmlComments(services, xmlDocFileName);
            }
        }

        private static void AddXmlComments(IServiceCollection services, string xmlDocFileName)
        {
            var serviceProvider = services.BuildServiceProvider();
            var hostEnv = serviceProvider.GetService<IHostingEnvironment>();

            if (hostEnv.IsDevelopment())
            {
                var platformService = PlatformServices.Default;
                var xmlDoc = Path.Combine(platformService.Application.ApplicationBasePath, xmlDocFileName);

                if (!File.Exists(xmlDoc))
                {
                    return; // ugly workaround as currently packaging does not pick the xml files (it used to in project.json era)
                }

                services.ConfigureSwaggerGen(options =>
                {
                    options.IncludeXmlComments(xmlDoc);
                });
            }
        }
    }
}