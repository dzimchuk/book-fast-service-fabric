using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using BookFast.ServiceFabric;

namespace BookFast.Booking
{
    internal sealed class BookingService : StatelessService
    {
        public BookingService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() =>
            new ServiceInstanceListener[]
            {
                ServiceInstanceListenerFactory.CreateKestrelListener(typeof(Startup), (serviceContext, message) => ServiceEventSource.Current.ServiceMessage(serviceContext, message))
            };
    }
}