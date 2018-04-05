using BookFast.Facility.QueryStack.Representations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.QueryStack
{
    public interface IAccommodationQueryDataSource
    {
        Task<AccommodationRepresentation> FindAsync(int id);
        Task<IEnumerable<AccommodationRepresentation>> ListAsync(int facilityId);
    }
}
