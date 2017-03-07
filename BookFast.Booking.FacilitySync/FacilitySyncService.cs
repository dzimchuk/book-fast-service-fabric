using System.Fabric;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace BookFast.Booking.FacilitySync
{
    internal sealed class FacilitySyncService : StatelessService
    {
        public FacilitySyncService(StatelessServiceContext context)
            : base(context)
        { }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    //await facilityDataService.SynchronizeAsync(cancellationToken);
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