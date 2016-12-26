using Microsoft.Extensions.Configuration;
using System.Fabric;

namespace BookFast.ServiceFabric.Configuration
{
    internal class ServiceFabricConfigurationSource : IConfigurationSource
    {
        private readonly ServiceContext serviceContext;

        public ServiceFabricConfigurationSource(ServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ServiceFabricConfigurationProvider(serviceContext);
        }
    }
}
