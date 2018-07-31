using BookFast.Booking.CommandStack.Commands;
using BookFast.Booking.CommandStack.Data;
using BookFast.Booking.Domain.Exceptions;
using BookFast.Booking.Domain.Models;
using BookFast.Security;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Booking.CommandStack.CommandHandlers
{
    public class BookAccommodationCommandHandler : IRequestHandler<BookAccommodationCommand, Guid>
    {
        private readonly IBookingRepository bookingRepository;
        private readonly ISecurityContext securityContext;
        private readonly IFacilityDataSource facilityDataSource;

        public BookAccommodationCommandHandler(IBookingRepository bookingRepository, ISecurityContext securityContext, IFacilityDataSource facilityDataSource)
        {
            this.bookingRepository = bookingRepository;
            this.securityContext = securityContext;
            this.facilityDataSource = facilityDataSource;
        }

        public async Task<Guid> Handle(BookAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodation = await facilityDataSource.FindAccommodationAsync(request.AccommodationId);
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(request.AccommodationId);
            }

            var facility = await facilityDataSource.FindFacilityAsync(accommodation.FacilityId);

            var bookingRecord = BookingRecord.NewBooking(accommodation, facility, request.FromDate, request.ToDate, securityContext);
            await bookingRepository.AddAsync(bookingRecord);

            return bookingRecord.Id;
        }
    }
}
