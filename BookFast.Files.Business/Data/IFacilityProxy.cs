using BookFast.Files.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace BookFast.Files.Business.Data
{
    public interface IFacilityProxy
    {
        Task<Facility> FindFacilityAsync(int facilityId);
        Task<Accommodation> FindAccommodationAsync(int accommodationId);
    }
}
