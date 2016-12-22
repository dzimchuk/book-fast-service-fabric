using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Swagger.Model;

namespace BookFast.Facility.Swagger
{
    internal static class SwaggerExtensions
    {
        public static void AddSwashbuckle(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Title = "Book Fast API",
                    Version = "v1"
                });

                options.OperationFilter<DefaultContentTypeOperationFilter>();

                options.DescribeAllEnumsAsStrings();
            });


            var serviceProvider = services.BuildServiceProvider();
            var hostEnv = serviceProvider.GetService<IHostingEnvironment>();

            if (hostEnv.IsDevelopment())
            {
                var platformService = PlatformServices.Default;
                var xmlDoc = $@"{platformService.Application.ApplicationBasePath}\BookFast.Facility.xml";

                services.ConfigureSwaggerGen(options =>
                {
                    options.IncludeXmlComments(xmlDoc);
                });
            }
        }
    }
}