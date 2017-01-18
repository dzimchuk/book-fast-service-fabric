using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Files.Business.Data;
using BookFast.Framework;
using BookFast.Files.Data.Mappers;

namespace BookFast.Files.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageOptions>(configuration.GetSection("Data:Azure:Storage"));
            services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();

            services.AddScoped<IFacilityMapper, FacilityMapper>();
            services.AddScoped<IFacilityProxy, FacilityProxy>();
        }
    }
}
