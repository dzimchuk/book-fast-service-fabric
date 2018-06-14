using BookFast.Api.Formatters;
using BookFast.Api.SecurityContext;
using BookFast.Api.Swagger;
using BookFast.Security;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

#pragma warning disable ET002 // Namespace does not match file path or default namespace
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore ET002 // Namespace does not match file path or default namespace
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSecurityContext(this IServiceCollection services)
        {
            services.AddScoped<ISecurityContext, SecurityContextProvider>();
        }

        public static void AddAndConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.OutputFormatters.Insert(0, new BusinessExceptionOutputFormatter());
            })
            .SetCompatibilityVersion(AspNetCore.Mvc.CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }

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

                options.SchemaFilter<SwaggerIgnoreSchemaFilter>();

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
