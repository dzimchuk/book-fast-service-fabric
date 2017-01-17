namespace BookFast.Booking.Data
{
    public interface IBookingMapper
    {
        Models.Booking MapFrom(Contracts.Models.Booking booking);
    }
}