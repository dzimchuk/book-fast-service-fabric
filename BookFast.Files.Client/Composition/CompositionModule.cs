using BookFast.Framework;
using BookFast.Rest;
using BookFast.ServiceFabric.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;

namespace BookFast.Files.Client.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiOptions>(configuration.GetSection("FilesApi"));

            services.AddSingleton(new FabricClient());

            services.AddSingleton<ICommunicationClientFactory<CommunicationClient<IBookFastFilesAPI>>>(
                serviceProvider => new FilesCommunicationClientFactory(
                    new ServicePartitionResolver(() => serviceProvider.GetService<FabricClient>()),
                    serviceProvider.GetService<IAccessTokenProvider>(),
                    serviceProvider.GetService<IOptions<ApiOptions>>()));

            services.AddSingleton<IPartitionClientFactory<CommunicationClient<IBookFastFilesAPI>>, FilesPartitionClientFactory>();
        }
    }
}
