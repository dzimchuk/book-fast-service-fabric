using System;
using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.Framework;

namespace BookFast.Facility.Data.Queries
{
    internal class FindFacilityQuery : IQuery<BookFastContext, Contracts.Models.Facility>
    {
        private readonly Guid facilityId;
        private readonly IFacilityMapper mapper;

        public FindFacilityQuery(Guid facilityId, IFacilityMapper mapper)
        {
            this.facilityId = facilityId;
            this.mapper = mapper;
        }

        public async Task<Contracts.Models.Facility> ExecuteAsync(BookFastContext model)
        {
            var facility = await model.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            return facility != null ? mapper.MapFrom(facility) : null;
        }
    }
}