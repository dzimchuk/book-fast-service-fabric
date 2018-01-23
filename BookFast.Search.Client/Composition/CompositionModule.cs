using BookFast.SeedWork;
using BookFast.ServiceFabric.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;
using BookFast.Rest;

namespace BookFast.Search.Client.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiOptions>(configuration.GetSection("SearchApi"));

            services.AddSingleton(new FabricClient());

            services.AddSingleton<ICommunicationClientFactory<CommunicationClient<IBookFastSearchAPI>>>(
                serviceProvider => new SearchCommunicationClientFactory(
                    new ServicePartitionResolver(() => serviceProvider.GetService<FabricClient>()),
                    serviceProvider.GetService<IApiClientFactory<IBookFastSearchAPI>>()));

            services.AddSingleton<IPartitionClientFactory<CommunicationClient<IBookFastSearchAPI>>, SearchPartitionClientFactory>();
            services.AddSingleton<IApiClientFactory<IBookFastSearchAPI>, SearchApiClientFactory>();
        }
    }
}
