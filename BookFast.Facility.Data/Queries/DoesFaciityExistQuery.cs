using System;
using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.Framework;

namespace BookFast.Facility.Data.Queries
{
    internal class DoesFaciityExistQuery : IQuery<BookFastContext, bool>
    {
        private readonly Guid facilityId;

        public DoesFaciityExistQuery(Guid facilityId)
        {
            this.facilityId = facilityId;
        }

        public Task<bool> ExecuteAsync(BookFastContext model)
        {
            return model.Facilities.AnyAsync(f => f.Id == facilityId);
        }
    }
}