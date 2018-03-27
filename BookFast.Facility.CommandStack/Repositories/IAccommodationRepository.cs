using BookFast.SeedWork.Modeling;
using System.Threading.Tasks;

namespace BookFast.Facility.CommandStack.Repositories
{
    public interface IAccommodationRepository : IRepository<Domain.Models.Accommodation>
    {
        Task<Domain.Models.Accommodation> FindAsync(int accommodationId);
        Task<int> CreateAsync(Domain.Models.Accommodation accommodation);
        Task UpdateAsync(Domain.Models.Accommodation accommodation);
        Task DeleteAsync(Domain.Models.Accommodation accommodation);
    }
}
