using System;
using System.Linq;
using BookFast.Booking.Contracts.Models;
using BookFast.Facility.Client.Models;
using AutoMapper;

namespace BookFast.Booking.Data.Mappers
{
    internal class FacilityMapper : IFacilityMapper
    {
        private static readonly IMapper Mapper;

        static FacilityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<AccommodationRepresentation, Accommodation>()
                .ConstructUsing(representation => new Accommodation
                {
                    Id = representation.Id ?? Guid.Empty,
                    FacilityId = representation.FacilityId ?? Guid.Empty,
                    Name = representation.Name,
                    Description = representation.Description,
                    RoomCount = representation.RoomCount ?? 0,
                    Images = representation.Images != null ? representation.Images.ToArray() : null
                });

                configuration.CreateMap<FacilityRepresentation, Contracts.Models.Facility>();
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public Contracts.Models.Facility MapFrom(FacilityRepresentation facility) => Mapper.Map<Contracts.Models.Facility>(facility);

        public Accommodation MapFrom(AccommodationRepresentation accommodation) => Mapper.Map<Accommodation>(accommodation);
    }
}
