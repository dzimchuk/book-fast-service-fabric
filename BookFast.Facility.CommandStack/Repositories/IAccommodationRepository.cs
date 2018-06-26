using BookFast.ReliableEvents.CommandStack;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.Repositories
{
    public interface IAccommodationRepository : IRepositoryWithReliableEvents<Domain.Models.Accommodation>
    {
        Task<Domain.Models.Accommodation> FindAsync(int id);
        Task<int> AddAsync(Domain.Models.Accommodation accommodation);
        Task UpdateAsync(Domain.Models.Accommodation accommodation);
        Task DeleteAsync(int id);
    }
}
