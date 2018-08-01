using BookFast.Booking.CommandStack.Commands;
using BookFast.Booking.CommandStack.Data;
using BookFast.Booking.Domain.Exceptions;
using BookFast.ReliableEvents.CommandStack;
using BookFast.Security;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Booking.CommandStack.CommandHandlers
{
    public class CancelBookingCommandHandler : AsyncRequestHandler<CancelBookingCommand>
    {
        private readonly IBookingRepository bookingRepository;
        private readonly ISecurityContext securityContext;
        private readonly CommandContext commandContext;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository, ISecurityContext securityContext, CommandContext commandContext)
        {
            this.bookingRepository = bookingRepository;
            this.securityContext = securityContext;
            this.commandContext = commandContext;
        }

        protected override async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingRecord = await bookingRepository.FindAsync(request.Id);
            if (bookingRecord == null)
            {
                throw new BookingNotFoundException(request.Id);
            }

            bookingRecord.Cancel(securityContext);

            await bookingRepository.UpdateAsync(bookingRecord);

            await bookingRepository.SaveChangesAsync(bookingRecord, commandContext);
        }
    }
}
