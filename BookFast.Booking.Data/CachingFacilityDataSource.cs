using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using BookFast.Booking.CommandStack.Data;
using BookFast.Booking.Domain.Models;

namespace BookFast.Booking.Data
{
    internal class CachingFacilityDataSource : IFacilityDataSource
    {
        private readonly IFacilityDataSource dataSource;
        private readonly IDistributedCache cache;

        public CachingFacilityDataSource(IFacilityDataSource dataSource, IDistributedCache cache)
        {
            this.dataSource = dataSource;
            this.cache = cache;
        }

        public async Task<Accommodation> FindAccommodationAsync(int accommodationId)
        {
            var key = $"Accoommodation_{accommodationId}";

            var cachedItem = await cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedItem))
            {
                return JsonConvert.DeserializeObject<Accommodation>(cachedItem);
            }

            var accommodation = await dataSource.FindAccommodationAsync(accommodationId);
            if (accommodation == null)
            {
                return null;
            }

            await cache.SetStringAsync(key, JsonConvert.SerializeObject(accommodation));
            return accommodation;
        }

        public async Task<Domain.Models.Facility> FindFacilityAsync(int facilityId)
        {
            var key = $"Facility_{facilityId}";

            var cachedItem = await cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedItem))
            {
                return JsonConvert.DeserializeObject<Domain.Models.Facility>(cachedItem);
            }

            var facility = await dataSource.FindFacilityAsync(facilityId);
            if (facility == null)
            {
                return null;
            }

            await cache.SetStringAsync(key, JsonConvert.SerializeObject(facility));
            return facility;
        }
    }
}
