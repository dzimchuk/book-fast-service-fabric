using System;
using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.SeedWork;

namespace BookFast.Facility.Data.Queries
{
    internal class DoesAccommodationExistQuery : IQuery<FacilityContext, bool>
    {
        private readonly Guid accommodationId;

        public DoesAccommodationExistQuery(Guid accommodationId)
        {
            this.accommodationId = accommodationId;
        }

        public Task<bool> ExecuteAsync(FacilityContext model)
        {
            return model.Accommodations.AnyAsync(a => a.Id == accommodationId);
        }
    }
}