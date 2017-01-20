using BookFast.Framework;
using BookFast.Rest;
using BookFast.Web.Contracts.Security;
using BookFast.Web.Features.Booking;
using BookFast.Web.Features.Facility;
using BookFast.Web.Features.Files;
using BookFast.Web.Infrastructure;
using BookFast.Web.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OdeToCode.AddFeatureFolders;
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();

            var featureOptions = new FeatureFolderOptions();
            services.AddMvc().AddFeatureFolders(featureOptions).AddRazorOptions(o => o.ViewLocationFormats.Add(featureOptions.FeatureNamePlaceholder + "/{1}/{0}.cshtml"));

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
            services.AddSingleton<FacilityMapper>();
            services.AddSingleton<AccommodationMapper>();
            services.AddSingleton<BookingMapper>();
            services.AddSingleton<FileAccessMapper>();
        }
    }
}