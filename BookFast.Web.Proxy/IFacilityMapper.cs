using System.Collections.Generic;
using BookFast.Web.Contracts.Models;
using BookFast.Web.Proxy.Models;

namespace BookFast.Web.Proxy
{
    public interface IFacilityMapper
    {
        List<Facility> MapFrom(IList<FacilityRepresentation> facilities);
        Facility MapFrom(FacilityRepresentation facility);
        FacilityData MapFrom(FacilityDetails details);
    }
}