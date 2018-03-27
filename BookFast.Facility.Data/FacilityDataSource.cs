using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Data.Commands;
using BookFast.Facility.Data.Models;
using BookFast.Facility.Data.Queries;
using BookFast.Facility.Business.Data;

namespace BookFast.Facility.Data
{
    internal class FacilityDataSource : IFacilityDataSource
    {
        private readonly FacilityContext context;
        private readonly IFacilityMapper mapper;

        public FacilityDataSource(FacilityContext context, IFacilityMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Task<List<Contracts.Models.Facility>> ListByOwnerAsync(string owner)
        {
            var query = new ListFacilitiesByOwnerQuery(owner, mapper);
            return query.ExecuteAsync(context);
        }

        public Task<Contracts.Models.Facility> FindAsync(Guid facilityId)
        {
            var query = new FindFacilityQuery(facilityId, mapper);
            return query.ExecuteAsync(context);
        }

        public Task CreateAsync(Contracts.Models.Facility facility)
        {
            var command = new CreateFacilityCommand(facility, mapper);
            return command.ApplyAsync(context);
        }

        public Task UpdateAsync(Contracts.Models.Facility facility)
        {
            var command = new UpdateFacilityCommand(facility, mapper);
            return command.ApplyAsync(context);
        }

        public Task<bool> ExistsAsync(Guid facilityId)
        {
            var query = new DoesFaciityExistQuery(facilityId);
            return query.ExecuteAsync(context);
        }

        public Task DeleteAsync(Guid facilityId)
        {
            var command = new DeleteFacilityCommand(facilityId);
            return command.ApplyAsync(context);
        }
    }
}