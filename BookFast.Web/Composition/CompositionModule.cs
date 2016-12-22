using BookFast.Common.Framework;
using BookFast.Web.Contracts.Security;
using BookFast.Web.Controllers;
using BookFast.Web.Infrastructure;
using BookFast.Web.Infrastructure.Authentication;
using BookFast.Web.Mappers;
using BookFast.Web.Proxy.RestClient;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BookFast.Web.Composition
{
    internal class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication:AzureAd"));
            services.Configure<B2CAuthenticationOptions>(configuration.GetSection("Authentication:AzureAd:B2C"));
            services.Configure<B2CPolicies>(configuration.GetSection("Authentication:AzureAd:B2C:Policies"));

            services.AddScoped<SecurityContext>();
            services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();

            services.AddMvc();

            RegisterAuthorizationPolicies(services);
            RegisterMappers(services);
        }

        private static void RegisterAuthorizationPolicies(IServiceCollection services)
        {
            services.AddAuthorization(
                options => options.AddPolicy("FacilityProviderOnly",
                    config => config.RequireRole(InteractorRole.FacilityProvider.ToString())));

            services.AddAuthorization(options => options.AddPolicy("Customer", 
                config => config.RequireClaim(BookFastClaimTypes.InteractorRole, new[] { InteractorRole.Customer.ToString() })));
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IAccommodationMapper, AccommodationMapper>();
            services.AddScoped<IBookingMapper, BookingMapper>();
            services.AddScoped<IFileAccessMapper, FileAccessMapper>();
        }
    }
}