using MediatR;
using System;

namespace BookFast.Booking.CommandStack.Commands
{
    public class CancelBookingCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
