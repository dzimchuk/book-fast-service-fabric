using BookFast.Facility.Domain.Events;
using BookFast.SeedWork.Modeling;
using System;

namespace BookFast.Facility.Domain.Models
{
    public class Accommodation : Entity<int>, IAggregateRoot
    {
        public int FacilityId { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int RoomCount { get; private set; }
        public string[] Images { get; private set; }

        public static Accommodation FromStorage(
            int id,
            int facilityId,
            string name,
            string description,
            int roomCount,
            string[] images)
        {
            return new Accommodation
            {
                Id = id,
                FacilityId = facilityId,
                Name = name ?? throw new ArgumentNullException(nameof(name)),
                Description = description,
                RoomCount = roomCount,
                Images = images
            };
        }

        public static Accommodation NewAccommodation(
            int facilityId,
            string name,
            string description,
            int roomCount,
            string[] images)
        {
            var accommodation = new Accommodation
            {
                FacilityId = facilityId,
                Name = name ?? throw new ArgumentNullException(nameof(name)),
                Description = description,
                RoomCount = roomCount,
                Images = ImagePathHelper.CleanUp(images)
            };

            accommodation.AddEvent(new AccommodationCreatedEvent(accommodation));

            return accommodation;
        }

        public void Update(
            string name,
            string description,
            int roomCount,
            string[] images)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            RoomCount = roomCount;
            Images = ImagePathHelper.Merge(Images, images);

            AddEvent(new AccommodationUpdatedEvent
            {
                Id = Id,
                FacilityId = FacilityId,
                Name = Name,
                Description = Description,
                RoomCount = RoomCount,
                Images = Images
            });
        }

        public void Delete()
        {
            AddEvent(new AccommodationDeletedEvent { Id = Id });
        }
    }
}