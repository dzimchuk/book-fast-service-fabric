using BookFast.Facility.Controllers;
using BookFast.Facility.Infrastructure;
using BookFast.Facility.Infrastructure.Authentication;
using BookFast.Facility.Mappers;
using BookFast.Facility.Swagger;
using BookFast.Common.Framework;
using BookFast.Common.Security;
using BookFast.Facility.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            RegisterApplicationServices(services);
            RegisterMappers(services);

            services.AddSwashbuckle();
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

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddScoped<ISecurityContext, SecurityContextProvider>();
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();
        }
    }
}