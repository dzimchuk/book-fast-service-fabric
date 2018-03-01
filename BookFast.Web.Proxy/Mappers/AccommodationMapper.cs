using System;
using System.Collections.Generic;
using AutoMapper;
using BookFast.Web.Contracts.Models;
using System.Linq;
using BookFast.Facility.Client.Models;

namespace BookFast.Web.Proxy.Mappers
{
    internal class AccommodationMapper : IAccommodationMapper
    {
        private static readonly IMapper Mapper;

        static AccommodationMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<AccommodationRepresentation, Accommodation>()
                             .ConvertUsing(representation => new Accommodation
                                                             {
                                                                 Id = representation.Id ?? Guid.Empty,
                                                                 FacilityId = representation.FacilityId ?? Guid.Empty,
                                                                 Details = new AccommodationDetails
                                                                           {
                                                                               Name = representation.Name,
                                                                               Description = representation.Description,
                                                                               RoomCount = representation.RoomCount ?? 0,
                                                                               Images = representation.Images != null ? representation.Images.ToArray() : null
                                                                           }
                                                             });
                configuration.CreateMap<AccommodationDetails, AccommodationData>()
                .ForMember(details => details.Images, 
                    config => config.ResolveUsing<ArrayToListResolver>());
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public Accommodation MapFrom(AccommodationRepresentation accommodation)
        {
            return Mapper.Map<Accommodation>(accommodation);
        }

        public List<Accommodation> MapFrom(IList<AccommodationRepresentation> accommodations)
        {
            return Mapper.Map<List<Accommodation>>(accommodations);
        }

        public AccommodationData MapFrom(AccommodationDetails details)
        {
            return Mapper.Map<AccommodationData>(details);
        }

        private class ArrayToListResolver : IValueResolver<AccommodationDetails, AccommodationData, IList<string>>
        {
            public IList<string> Resolve(AccommodationDetails source, AccommodationData destination, IList<string> destMember, ResolutionContext context)
            {
                return source.Images?.ToList();
            }
        }
    }
}