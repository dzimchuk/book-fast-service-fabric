using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Booking.Contracts
{
    public interface IFacilityDataService
    {
        Task SynchronizeAsync(CancellationToken cancellationToken);
    }
}
