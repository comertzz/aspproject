using aspproject.Models;

namespace aspproject.Services;

public interface IBookingService
{
    Booking CreateBooking(BookingCheckoutRequest request);
    Booking? GetBookingByCode(string bookingCode);
    Booking? FindBooking(string? bookingCode, string? contactInfo);
    bool CancelBooking(string bookingCode);
    IReadOnlyList<Booking> GetAllBookings();
}
