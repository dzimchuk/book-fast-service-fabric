using BookFast.Facility.QueryStack;
using BookFast.Facility.QueryStack.Representations;
using BookFast.Security;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.Facility.Data.Queries
{
    internal class FacilityQueryDataSource : IFacilityQueryDataSource
    {
        private readonly FacilityContext context;
        private readonly ISecurityContext securityContext;

        public FacilityQueryDataSource(FacilityContext context, ISecurityContext securityContext)
        {
            this.context = context;
            this.securityContext = securityContext;
        }

        public async Task<FacilityRepresentation> FindAsync(int id)
        {
            var facility = await (from item in context.Facilities.AsNoTracking()
                                  where item.Id == id
                                  select new
                                  {
                                      item.Id,
                                      item.Name,
                                      item.Description,
                                      item.StreetAddress,
                                      item.Latitude,
                                      item.Longitude,
                                      item.Images,
                                      AccommodationCount = item.Accommodations.Count()
                                  }).FirstOrDefaultAsync();

            return facility != null
                ? new FacilityRepresentation
                  {
                      Id = facility.Id,
                      Name = facility.Name,
                      Description = facility.Description,
                      StreetAddress = facility.StreetAddress,
                      Latitude = facility.Latitude,
                      Longitude = facility.Longitude,
                      Images = facility.Images.ToStringArray(),
                      AccommodationCount = facility.AccommodationCount
                  }
                : null;
        }

        public async Task<IEnumerable<FacilityRepresentation>> ListAsync()
        {
            var facilities = await (from item in context.Facilities.AsNoTracking()
                                    where item.Owner == securityContext.GetCurrentTenant()
                                    select new
                                    {
                                        item.Id,
                                        item.Name,
                                        item.Description,
                                        item.StreetAddress,
                                        item.Latitude,
                                        item.Longitude,
                                        item.Images,
                                        AccommodationCount = item.Accommodations.Count()
                                    }).ToListAsync();

            return (from facility in facilities
                    select new FacilityRepresentation
                    {
                        Id = facility.Id,
                        Name = facility.Name,
                        Description = facility.Description,
                        StreetAddress = facility.StreetAddress,
                        Latitude = facility.Latitude,
                        Longitude = facility.Longitude,
                        Images = facility.Images.ToStringArray(),
                        AccommodationCount = facility.AccommodationCount
                    }).ToList();
        }
    }
}
