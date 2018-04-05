using BookFast.SeedWork.Modeling;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.Repositories
{
    public interface IAccommodationRepository : IRepository<Domain.Models.Accommodation>
    {
        Task<Domain.Models.Accommodation> FindAsync(int id);
        Task<int> CreateAsync(Domain.Models.Accommodation accommodation);
        Task UpdateAsync(Domain.Models.Accommodation accommodation);
        Task DeleteAsync(int id);
    }
}
