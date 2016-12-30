using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using BookFast.ServiceFabric;

namespace BookFast.Facility
{
    internal sealed class FacilityService : StatelessService
    {
        public FacilityService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() => 
            new ServiceInstanceListener[]
            {
                ServiceInstanceListenerFactory.CreateListener(typeof(Startup), (serviceContext, message) => ServiceEventSource.Current.ServiceMessage(serviceContext, message))
            };
    }
}