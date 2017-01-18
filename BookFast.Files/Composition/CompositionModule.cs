using BookFast.Files.Controllers;
using BookFast.Files.Mappers;
using BookFast.Framework;
using BookFast.Security;
using BookFast.Security.AspNetCore.Authentication;
using BookFast.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookFast.Files.Composition
{
    internal class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication:AzureAd"));

            services.AddMvc();

            RegisterAuthorizationPolicies(services);

            services.AddScoped<IFileAccessMapper, FileAccessMapper>();

            services.AddSwashbuckle("Book Fast Files API", "v1", "BookFast.Files.xml");
        }

        private static void RegisterAuthorizationPolicies(IServiceCollection services)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("Facility.Write", config =>
                                                        {
                                                            config.RequireRole(InteractorRole.FacilityProvider.ToString(), InteractorRole.ImporterProcess.ToString());
                                                        });
                });
        }
    }
}