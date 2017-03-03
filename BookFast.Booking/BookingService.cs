using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using BookFast.ServiceFabric;
using BookFast.Booking.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace BookFast.Booking
{
    internal sealed class BookingService : StatefulService
    {
        private readonly IFacilityDataService facilityDataService;

        public BookingService(StatefulServiceContext context, IFacilityDataService facilityDataService)
            : base(context)
        {
            this.facilityDataService = facilityDataService;
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() =>
            new ServiceReplicaListener[]
            {
                ServiceReplicaListenerFactory.CreateKestrelListener(typeof(Startup), StateManager, (serviceContext, message) => ServiceEventSource.Current.ServiceMessage(serviceContext, message))
            };

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await facilityDataService.SynchronizeAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.ServiceMessage(Context, "Error synchronizing facility data. Details: {0}", ex.ToString());
                }

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}