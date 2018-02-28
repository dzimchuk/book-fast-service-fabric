using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace BookFast.Swagger
{
    public static class SwaggerExtensions
    {
        public static void AddSwashbuckle(this IServiceCollection services, IConfiguration configuration, string title, string version, string xmlDocFileName = null)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(version, new Info
                {
                    Title = title,
                    Version = version
                });

                options.OperationFilter<DefaultContentTypeOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();

                options.DescribeAllEnumsAsStrings();

                //options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Flow = "implicit",
                //    AuthorizationUrl = "https://login.microsoftonline.com/common/oauth2/authorize"
                //});
            });

            if (!string.IsNullOrWhiteSpace(xmlDocFileName))
            {
                AddXmlComments(services, xmlDocFileName);
            }
        }

        public static void UseSwagger(this IApplicationBuilder app, string title, string version)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint($"/api-docs/{version}/swagger.json", $"{title} {version}");
            //    options.RoutePrefix = "api-docs";
            //});
        }

        private static void AddXmlComments(IServiceCollection services, string xmlDocFileName)
        {
            var xmlDoc = Path.Combine(AppContext.BaseDirectory, xmlDocFileName);

            if (!File.Exists(xmlDoc))
            {
                return;
            }

            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments(xmlDoc);
            });
        }
    }
}