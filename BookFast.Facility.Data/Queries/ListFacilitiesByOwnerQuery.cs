using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BookFast.Facility.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.SeedWork;

namespace BookFast.Facility.Data.Queries
{
    internal class ListFacilitiesByOwnerQuery : IQuery<FacilityContext, List<Contracts.Models.Facility>>
    {
        private readonly string owner;
        private readonly IFacilityMapper mapper;

        public ListFacilitiesByOwnerQuery(string owner, IFacilityMapper mapper)
        {
            this.owner = owner;
            this.mapper = mapper;
        }

        public async Task<List<Contracts.Models.Facility>> ExecuteAsync(FacilityContext model)
        {
            var facilities = await (from f in model.Facilities
                                    where f.Owner == owner
                                    select f).ToListAsync();

            return mapper.MapFrom(facilities).ToList();
        }
    }
}