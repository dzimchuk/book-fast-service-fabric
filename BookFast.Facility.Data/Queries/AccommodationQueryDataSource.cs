using BookFast.Facility.QueryStack;
using BookFast.Facility.QueryStack.Representations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookFast.Facility.Data.Queries
{
    internal class AccommodationQueryDataSource : IAccommodationQueryDataSource
    {
        private readonly FacilityContext context;

        public AccommodationQueryDataSource(FacilityContext context)
        {
            this.context = context;
        }

        public async Task<AccommodationRepresentation> FindAsync(int id)
        {
            var accommodation = await context.Accommodations.FirstOrDefaultAsync(item => item.Id == id);
            return accommodation != null
                ? new AccommodationRepresentation
                  {
                      Id = accommodation.Id,
                      FacilityId = accommodation.FacilityId,
                      Name = accommodation.Name,
                      Description = accommodation.Description,
                      RoomCount = accommodation.RoomCount,
                      Images = string.IsNullOrWhiteSpace(accommodation.Images) ? null : JsonConvert.DeserializeObject<string[]>(accommodation.Images)
                }
                : null;
        }

        public Task<IEnumerable<AccommodationRepresentation>> ListAsync(int facilityId)
        {
            throw new NotImplementedException();
        }
    }
}
