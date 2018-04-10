using AutoMapper;
using BookFast.Facility.Client.Models;
using BookFast.Web.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

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
                                                                 Id = representation.Id,
                                                                 FacilityId = representation.FacilityId,
                                                                 Details = new AccommodationDetails
                                                                           {
                                                                               Name = representation.Name,
                                                                               Description = representation.Description,
                                                                               RoomCount = representation.RoomCount,
                                                                               Images = representation.Images?.ToArray()
                                                                           }
                                                             });
                configuration.CreateMap<AccommodationDetails, CreateAccommodationCommand>()
                    .ForMember(command => command.FacilityId, config => config.Ignore())
                    .ForMember(command => command.Images, config => config.ResolveUsing<ArrayToListResolver>());
                configuration.CreateMap<AccommodationDetails, UpdateAccommodationCommand>()
                    .ForMember(command => command.AccommodationId, config => config.Ignore())
                    .ForMember(command => command.Images, config => config.ResolveUsing<ArrayToListResolver>());
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

        public CreateAccommodationCommand ToCreateCommand(AccommodationDetails details)
        {
            return Mapper.Map<CreateAccommodationCommand>(details);
        }

        public UpdateAccommodationCommand ToUpdateCommand(AccommodationDetails details)
        {
            return Mapper.Map<UpdateAccommodationCommand>(details);
        }

        private class ArrayToListResolver : 
            IValueResolver<AccommodationDetails, CreateAccommodationCommand, IList<string>>,
            IValueResolver<AccommodationDetails, UpdateAccommodationCommand, IList<string>>
        {
            public IList<string> Resolve(AccommodationDetails source, CreateAccommodationCommand destination, IList<string> destMember, ResolutionContext context)
            {
                return source.Images?.ToList();
            }

            public IList<string> Resolve(AccommodationDetails source, UpdateAccommodationCommand destination, IList<string> destMember, ResolutionContext context)
            {
                return source.Images?.ToList();
            }
        }
    }
}