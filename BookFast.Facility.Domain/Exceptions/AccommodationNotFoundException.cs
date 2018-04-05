﻿using BookFast.SeedWork;
using System;

namespace BookFast.Facility.Domain.Exceptions
{
    public class AccommodationNotFoundException : FormattedException
    {
        public int AccommodationId { get; }

        public AccommodationNotFoundException(int accommodationId)
            : base(ErrorCodes.AccommodationNotFound, $"Accommodation {accommodationId} not found.")
        {
            AccommodationId = accommodationId;
        }
    }
}