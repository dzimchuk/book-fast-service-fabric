using BookFast.Facility.Domain.Events;
using BookFast.Facility.Domain.Exceptions;
using BookFast.SeedWork.Modeling;
using System;

namespace BookFast.Facility.Domain.Models
{
    public class Facility : Entity<int>, IAggregateRoot
    {
        public string Owner { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string StreetAddress { get; private set; }
        public string[] Images { get; private set; }

        public Location Location { get; private set; }

        public int AccommodationCount { get; private set; }

        private Facility()
        {
        }

        public static Facility FromStorage(int id,
            string owner,
            string name,
            string description,
            string streetAddress,
            double? latitude,
            double? longitude,
            string[] images,
            int accommodationCount)
        {
            return new Facility
            {
                Id = id,
                Owner = owner,
                Name = name,
                Description = description,
                StreetAddress = streetAddress,
                Location = (latitude != null && longitude != null) ? new Location(latitude.Value, longitude.Value) : null,
                Images = images,
                AccommodationCount = accommodationCount
            };
        }

        public static Facility NewFacility(string owner,
            string name,
            string description,
            string streetAddress,
            double? latitude,
            double? longitude,
            string[] images)
        {
            var facility = new Facility
            {
                Owner = owner ?? throw new ArgumentNullException(nameof(owner)),
                Name = name ?? throw new ArgumentNullException(nameof(name)),
                Description = description,
                StreetAddress = streetAddress ?? throw new ArgumentNullException(streetAddress),
                Location = (latitude != null && longitude != null) ? new Location(latitude.Value, longitude.Value) : null,
                Images = ImagePathHelper.CleanUp(images)
            };

            facility.AddEvent(new FacilityCreatedEvent(facility));

            return facility;
        }

        public void Update(
            string name,
            string description,
            string streetAddress,
            double? latitude,
            double? longitude,
            string[] images)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            StreetAddress = streetAddress ?? throw new ArgumentNullException(streetAddress);
            Location = (latitude != null && longitude != null) ? new Location(latitude.Value, longitude.Value) : null;
            Images = ImagePathHelper.Merge(Images, images);

            AddEvent(new FacilityUpdatedEvent
            {
                Id = Id,
                Name = Name,
                Description = Description,
                StreetAddress = StreetAddress,
                Owner = Owner,
                Latitude = latitude,
                Longitude = longitude,
                Images = Images
            });
        }

        public void Delete()
        {
            if (AccommodationCount > 0)
            {
                throw new FacilityNotEmptyException(Id, AccommodationCount);
            }

            AddEvent(new FacilityDeletedEvent { Id = Id });
        }
    }
}