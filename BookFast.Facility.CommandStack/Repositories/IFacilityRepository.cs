using BookFast.ReliableEvents.CommandStack;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.Repositories
{
    public interface IFacilityRepository : IRepositoryWithReliableEvents<Domain.Models.Facility>
    {
        Task<Domain.Models.Facility> FindAsync(int id);
        Task<int> AddAsync(Domain.Models.Facility facility);
        Task UpdateAsync(Domain.Models.Facility facility);
        Task DeleteAsync(int id);
    }
}
