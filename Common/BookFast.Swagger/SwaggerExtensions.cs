using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Swagger.Model;

namespace BookFast.Swagger
{
    public static class SwaggerExtensions
    {
        public static void AddSwashbuckle(this IServiceCollection services, string title, string version, string xmlDocFileName)
        {
            services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
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
                var xmlDoc = $@"{platformService.Application.ApplicationBasePath}\{xmlDocFileName}";

                services.ConfigureSwaggerGen(options =>
                {
                    options.IncludeXmlComments(xmlDoc);
                });
            }
        }
    }
}