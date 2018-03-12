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
        
        public static Facility FromStorage(int id,
            string owner,
            string name,
            string description,
            string streetAddress,
            string[] images,
            Location location,
            int accommodationCount)
        {
            return new Facility
            {
                Id = id,
                Owner = owner,
                Name = name,
                Description = description,
                StreetAddress = streetAddress,
                Images = images,
                Location = location,
                AccommodationCount = accommodationCount
            };
        }

        public static Facility NewFacility(string owner,
            string name,
            string description,
            string streetAddress)
        {
            var facility = new Facility
            {
                Owner = owner ?? throw new ArgumentNullException(nameof(owner)),
                Name = name ?? throw new ArgumentNullException(nameof(name)),
                Description = description,
                StreetAddress = streetAddress ?? throw new ArgumentNullException(streetAddress)
            };

            facility.AddEvent(new FacilityCreatedEvent(facility));

            return facility;
        }

        public void Update(
            string name,
            string description,
            string streetAddress,
            string[] images)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            StreetAddress = streetAddress ?? throw new ArgumentNullException(streetAddress);
            Images = images;

            AddEvent(new FacilityUpdatedEvent(this));
        }

        public void Delete()
        {
            if (AccommodationCount > 0)
            {
                throw new FacilityNotEmptyException(Id, AccommodationCount);
            }

            AddEvent(new FacilityDeletedEvent(Id));
        }

        public void IncrementAccommodationCount()
        {
            AccommodationCount++;
        }

        public void DecrementAccommodationCound()
        {
            if (AccommodationCount > 0)
            {
                AccommodationCount--;
            }
        }
    }
}