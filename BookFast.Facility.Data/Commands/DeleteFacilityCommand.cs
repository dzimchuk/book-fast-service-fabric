using System;
using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookFast.Common.Framework;

namespace BookFast.Facility.Data.Commands
{
    internal class DeleteFacilityCommand : ICommand<BookFastContext>
    {
        private readonly Guid facilityId;

        public DeleteFacilityCommand(Guid facilityId)
        {
            this.facilityId = facilityId;
        }

        public async Task ApplyAsync(BookFastContext model)
        {
            var facility = await model.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility != null)
            {
                model.Facilities.Remove(facility);
                await model.SaveChangesAsync();
            }
        }
    }
}