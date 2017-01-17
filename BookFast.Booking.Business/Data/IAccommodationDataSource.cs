using System;
using System.Threading.Tasks;

namespace BookFast.Booking.Business.Data
{
    public interface IAccommodationDataSource
    {
        Task CheckAccommodationAsync(Guid accommodationId);
    }
}
