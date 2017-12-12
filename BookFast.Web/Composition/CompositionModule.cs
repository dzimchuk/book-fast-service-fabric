using BookFast.Framework;
using BookFast.Rest;
using BookFast.Web.Contracts.Security;
using BookFast.Web.Features.Booking;
using BookFast.Web.Features.Facility;
using BookFast.Web.Features.Files;
using BookFast.Web.Infrastructure.Authentication;
using BookFast.Web.Infrastructure.Authentication.Customer;
using BookFast.Web.Infrastructure.Authentication.Organizational;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OdeToCode.AddFeatureFolders;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BookFast.Web.Composition
{
    internal class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
            services.AddSingleton<ICustomerAccessTokenProvider, CustomerAccessTokenProvider>();

            var featureOptions = new FeatureFolderOptions();
            services.AddMvc(options => options.Filters.Add(typeof(ReauthenticationRequiredFilter)))
                .AddFeatureFolders(featureOptions).AddRazorOptions(o => o.ViewLocationFormats.Add(featureOptions.FeatureNamePlaceholder + "/{1}/{0}.cshtml"));

            RegisterAuthorizationPolicies(services);
            RegisterMappers(services);

            services.AddDistributedRedisCache(redisCacheOptions =>
            {
                redisCacheOptions.Configuration = configuration["Redis:Configuration"];
                redisCacheOptions.InstanceName = configuration["Redis:InstanceName"];
            });

            AddAuthentication(services, configuration);
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication:AzureAd"));
            services.Configure<B2CAuthenticationOptions>(configuration.GetSection("Authentication:AzureAd:B2C"));
            services.Configure<B2CPolicies>(configuration.GetSection("Authentication:AzureAd:B2C:Policies"));

            var serviceProvider = services.BuildServiceProvider();

            var authOptions = serviceProvider.GetService<IOptions<AuthenticationOptions>>();
            var b2cAuthOptions = serviceProvider.GetService<IOptions<B2CAuthenticationOptions>>();
            var b2cPolicies = serviceProvider.GetService<IOptions<B2CPolicies>>();

            var distributedCache = serviceProvider.GetService<IDistributedCache>();
            services.AddSingleton(distributedCache);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = B2CAuthConstants.OpenIdConnectB2CAuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnectB2CAuthentication(b2cAuthOptions.Value, b2cPolicies.Value, distributedCache)
            .AddOpenIdConnectOrganizationalAuthentication(authOptions.Value, distributedCache);
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