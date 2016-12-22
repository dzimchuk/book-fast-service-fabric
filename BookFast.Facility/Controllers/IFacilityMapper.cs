using System.Collections.Generic;
using BookFast.Facility.Models;
using BookFast.Facility.Models.Representations;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Controllers
{
    public interface IFacilityMapper
    {
        FacilityRepresentation MapFrom(Contracts.Models.Facility facility);
        IEnumerable<FacilityRepresentation> MapFrom(IEnumerable<Contracts.Models.Facility> facilities);
        FacilityDetails MapFrom(FacilityData data);
    }
}