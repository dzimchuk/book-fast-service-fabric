using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Models;

namespace BookFast.Web.Contracts
{
    public interface IAccommodationProxy
    {
        Task<List<Accommodation>> ListAsync(int facilityId);
        Task<Accommodation> FindAsync(int accommodationId);
        Task CreateAsync(int facilityId, AccommodationDetails details);
        Task UpdateAsync(int accommodationId, AccommodationDetails details);
        Task DeleteAsync(int accommodationId);
    }
}