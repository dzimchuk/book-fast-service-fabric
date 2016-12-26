using Microsoft.Extensions.Configuration;
using System.Fabric;

namespace BookFast.ServiceFabric.Configuration
{
    internal class ServiceFabricConfigurationProvider : ConfigurationProvider
    {
        private readonly ServiceContext serviceContext;

        public ServiceFabricConfigurationProvider(ServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        public override void Load()
        {
            var config = serviceContext.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            foreach (var section in config.Settings.Sections)
            {
                foreach (var parameter in section.Parameters)
                {
                    Data[$"{section.Name}{ConfigurationPath.KeyDelimiter}{parameter.Name}"] = parameter.Value;
                }
            }
        }
    }
}
