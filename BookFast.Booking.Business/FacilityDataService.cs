using BookFast.Booking.Business.Data;
using BookFast.Booking.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Booking.Business
{
    internal class FacilityDataService : IFacilityDataService
    {
        private readonly IFacilityDataSource dataSource;
        private readonly IFacilityProxy proxy;

        public FacilityDataService(IFacilityDataSource dataSource, IFacilityProxy proxy)
        {
            this.dataSource = dataSource;
            this.proxy = proxy;
        }

        public async Task SynchronizeAsync(CancellationToken cancellationToken)
        {
            var facilities = await proxy.ListFacilitiesAsync(cancellationToken);
            await dataSource.UpdateFacilitiesAsync(facilities);

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var facility in facilities)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var accommodations = await proxy.ListAccommodationsAsync(facility.Id, cancellationToken);
                await dataSource.UpdateAccommodationsAsync(accommodations);
            }
        }
    }
}
