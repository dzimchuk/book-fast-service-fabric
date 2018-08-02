using BookFast.Booking.Domain.Models;
using System.Threading.Tasks;

namespace BookFast.Booking.CommandStack.Data
{
    public interface IFacilityDataSource
    {
        Task<Accommodation> FindAccommodationAsync(int accommodationId);
        Task<Facility> FindFacilityAsync(int facilityId);
    }
}
