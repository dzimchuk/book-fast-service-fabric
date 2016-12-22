using System.Collections.Generic;

namespace BookFast.Facility.Data
{
    public interface IFacilityMapper
    {
        Contracts.Models.Facility MapFrom(Models.Facility facility);
        IEnumerable<Contracts.Models.Facility> MapFrom(IEnumerable<Models.Facility> facilities);
        Models.Facility MapFrom(Contracts.Models.Facility facility);
    }
}