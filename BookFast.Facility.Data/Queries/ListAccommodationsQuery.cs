using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Accommodation = BookFast.Facility.Contracts.Models.Accommodation;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookFast.SeedWork;

namespace BookFast.Facility.Data.Queries
{
    internal class ListAccommodationsQuery : IQuery<BookFastContext, List<Accommodation>>
    {
        private readonly Guid facilityId;
        private readonly IAccommodationMapper mapper;

        public ListAccommodationsQuery(Guid facilityId, IAccommodationMapper mapper)
        {
            this.facilityId = facilityId;
            this.mapper = mapper;
        }

        public async Task<List<Accommodation>> ExecuteAsync(BookFastContext model)
        {
            var accommodations = await model.Accommodations.Where(a => a.FacilityId == facilityId).ToListAsync();
            return mapper.MapFrom(accommodations).ToList();
        }
    }
}