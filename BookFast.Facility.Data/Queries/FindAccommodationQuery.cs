using System;
using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Accommodation = BookFast.Facility.Contracts.Models.Accommodation;
using Microsoft.EntityFrameworkCore;
using BookFast.Framework;

namespace BookFast.Facility.Data.Queries
{
    internal class FindAccommodationQuery : IQuery<BookFastContext, Accommodation>
    {
        private readonly Guid accommodationId;
        private readonly IAccommodationMapper mapper;

        public FindAccommodationQuery(Guid accommodationId, IAccommodationMapper mapper)
        {
            this.accommodationId = accommodationId;
            this.mapper = mapper;
        }

        public async Task<Accommodation> ExecuteAsync(BookFastContext model)
        {
            var accommodation = await model.Accommodations.FirstOrDefaultAsync(a => a.Id == accommodationId);
            return accommodation != null ? mapper.MapFrom(accommodation) : null;
        }
    }
}