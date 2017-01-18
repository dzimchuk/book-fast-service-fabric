using System;
using BookFast.Facility.Client.Models;
using BookFast.Files.Contracts.Models;
using AutoMapper;

namespace BookFast.Files.Data.Mappers
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
                    FacilityId = representation.FacilityId ?? Guid.Empty
                });

                configuration.CreateMap<FacilityRepresentation, Contracts.Models.Facility>()
                .ConstructUsing(representation => new Contracts.Models.Facility
                {
                    Id = representation.Id ?? Guid.Empty
                });
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public Contracts.Models.Facility MapFrom(FacilityRepresentation facility) => 
            Mapper.Map<Contracts.Models.Facility>(facility);

        public Accommodation MapFrom(AccommodationRepresentation accommodation) => 
            Mapper.Map<Accommodation>(accommodation);
    }
}
