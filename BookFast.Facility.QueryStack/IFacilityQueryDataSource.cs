using BookFast.Facility.QueryStack.Representations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.QueryStack
{
    public interface IFacilityQueryDataSource
    {
        Task<FacilityRepresentation> FindAsync(int id);
        Task<IEnumerable<FacilityRepresentation>> ListAsync();
    }
}
