using BookFast.SeedWork.Modeling;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.Repositories
{
    public interface IFacilityRepository : IRepository<Domain.Models.Facility>
    {
        Task<Domain.Models.Facility> FindAsync(int facilityId);
        Task<int> CreateAsync(Domain.Models.Facility facility);
        Task UpdateAsync(Domain.Models.Facility facility);
        Task DeleteAsync(Domain.Models.Facility facility);
    }
}
