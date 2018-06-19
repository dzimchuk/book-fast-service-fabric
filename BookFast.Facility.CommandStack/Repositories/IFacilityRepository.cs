using BookFast.SeedWork.Modeling;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.Repositories
{
    public interface IFacilityRepository : IRepository<Domain.Models.Facility>
    {
        Task<Domain.Models.Facility> FindAsync(int id);
        Task<int> AddAsync(Domain.Models.Facility facility);
        Task UpdateAsync(Domain.Models.Facility facility);
        Task DeleteAsync(int id);
    }
}
