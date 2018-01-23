using BookFast.SeedWork;
using BookFast.Rest;
using BookFast.ServiceFabric.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;

namespace BookFast.Facility.Client.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiOptions>(configuration.GetSection("FacilityApi"));

            services.AddSingleton(new FabricClient());

            services.AddSingleton<ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>>>(
                serviceProvider => new FacilityCommunicationClientFactory(
                    new ServicePartitionResolver(() => serviceProvider.GetService<FabricClient>()),
                    serviceProvider.GetService<IApiClientFactory<IBookFastFacilityAPI>>()));

            services.AddSingleton<IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>>, FacilityPartitionClientFactory>();
            services.AddSingleton<IApiClientFactory<IBookFastFacilityAPI>, FacilityApiClientFactory>();
        }
    }
}
