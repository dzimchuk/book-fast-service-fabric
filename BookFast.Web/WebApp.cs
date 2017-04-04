using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using BookFast.ServiceFabric;

namespace BookFast.Web
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class WebApp : StatelessService
    {
        public WebApp(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() =>
            new ServiceInstanceListener[]
            {
                ServiceInstanceListenerFactory.CreateExternalListener(typeof(Startup), (serviceContext, message) => ServiceEventSource.Current.ServiceMessage(serviceContext, message))
            };
    }
}