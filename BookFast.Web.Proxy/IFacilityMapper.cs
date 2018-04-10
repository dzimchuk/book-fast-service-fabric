using System.Collections.Generic;
using BookFast.Web.Contracts.Models;
using BookFast.Facility.Client.Models;

namespace BookFast.Web.Proxy
{
    public interface IFacilityMapper
    {
        List<Contracts.Models.Facility> MapFrom(IList<FacilityRepresentation> facilities);
        Contracts.Models.Facility MapFrom(FacilityRepresentation facility);
        CreateFacilityCommand ToCreateCommand(FacilityDetails details);
        UpdateFacilityCommand ToUpdateCommand(FacilityDetails details);
    }
}