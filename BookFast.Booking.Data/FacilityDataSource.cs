using BookFast.Booking.Business.Data;
using System;
using System.Threading.Tasks;
using BookFast.Booking.Contracts.Models;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Collections.Generic;

namespace BookFast.Booking.Data
{
    internal class FacilityDataSource : IFacilityDataSource
    {
        private readonly IReliableStateManager stateManager;

        public FacilityDataSource(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task<Accommodation> FindAccommodationAsync(Guid accommodationId)
        {
            ConditionalValue<Accommodation> result;

            var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Accommodation>>(Constants.AccommodationsDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                result = await dictionary.TryGetValueAsync(transaction, accommodationId);
                await transaction.CommitAsync();
            }

            return result.HasValue ? result.Value : null;
        }

        public async Task<Contracts.Models.Facility> FindFacilityAsync(Guid facilityId)
        {
            ConditionalValue<Contracts.Models.Facility> result;

            var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Contracts.Models.Facility>>(Constants.FacilitiesDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                result = await dictionary.TryGetValueAsync(transaction, facilityId);
                await transaction.CommitAsync();
            }

            return result.HasValue ? result.Value : null;
        }

        public async Task UpdateAccommodationsAsync(List<Accommodation> accommodations)
        {
            var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Accommodation>>(Constants.AccommodationsDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                foreach (var accommodation in accommodations)
                {
                    await dictionary.AddOrUpdateAsync(transaction, accommodation.Id, accommodation, (id, existing) => accommodation);
                }

                await transaction.CommitAsync();
            }
        }

        public async Task UpdateFacilitiesAsync(List<Contracts.Models.Facility> facilities)
        {
            var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Contracts.Models.Facility>>(Constants.FacilitiesDictionary);
            using (var transaction = stateManager.CreateTransaction())
            {
                foreach (var facility in facilities)
                {
                    await dictionary.AddOrUpdateAsync(transaction, facility.Id, facility, (id, existing) => facility);
                }

                await transaction.CommitAsync();
            }
        }
    }
}
