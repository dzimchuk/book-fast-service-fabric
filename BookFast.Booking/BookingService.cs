using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using BookFast.ServiceFabric;

namespace BookFast.Booking
{
    internal sealed class BookingService : StatefulService
    {
        public BookingService(StatefulServiceContext context)
            : base(context)
        {
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() =>
            new ServiceReplicaListener[]
            {
                ServiceReplicaListenerFactory.CreateListener(typeof(Startup), StateManager, (serviceContext, message) => ServiceEventSource.Current.ServiceMessage(serviceContext, message))
            };
    }
}