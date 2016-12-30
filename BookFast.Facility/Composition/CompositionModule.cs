using BookFast.Facility.Controllers;
using BookFast.Facility.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Framework;
using BookFast.Security.AspNetCore.Authentication;
using BookFast.Security;
using BookFast.Swagger;

namespace BookFast.Facility.Composition
{
    internal class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication:AzureAd"));
            services.Configure<B2CAuthenticationOptions>(configuration.GetSection("Authentication:AzureAd:B2C"));

            services.AddMvc();

            RegisterAuthorizationPolicies(services);
            RegisterMappers(services);

            services.AddSwashbuckle("Book Fast Facility API", "v1", "BookFast.Facility.xml");
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

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();
        }
    }
}