using aspproject.Data;
using aspproject.Models;
using Microsoft.EntityFrameworkCore;

namespace aspproject.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext context;
    private readonly IVehicleCatalogService vehicleCatalogService;

    public BookingService(AppDbContext context, IVehicleCatalogService vehicleCatalogService)
    {
        this.context = context;
        this.vehicleCatalogService = vehicleCatalogService;
    }

    public Booking CreateBooking(BookingCheckoutRequest request)
    {
        var vehicle = vehicleCatalogService.GetVehicleById(request.VehicleId)
            ?? throw new InvalidOperationException($"Vehicle with id {request.VehicleId} not found.");

        var pickupLoc = vehicleCatalogService.GetLocationById(request.PickupLocationId)
            ?? vehicleCatalogService.GetLocations().First();

        var returnLocId = request.ReturnLocationId > 0 ? request.ReturnLocationId : request.PickupLocationId;
        var returnLoc = vehicleCatalogService.GetLocationById(returnLocId) ?? pickupLoc;

        int totalDays = Math.Max(1, request.ReturnDate.DayNumber - request.PickupDate.DayNumber);
        decimal vehicleTotal = vehicle.DailyPrice * totalDays;

        decimal extrasTotal = 0m;
        if (request.HasFullInsurance) extrasTotal += 300m * totalDays;
        if (request.HasAdditionalDriver) extrasTotal += 150m * totalDays;
        if (request.HasChildSeat) extrasTotal += 120m * totalDays;
        if (request.HasTollPass) extrasTotal += 350m;

        string code;
        do
        {
            code = $"REZ-{Random.Shared.Next(10000, 99999)}";
        } while (context.Bookings.Any(b => b.BookingCode == code));

        var booking = new Booking
        {
            BookingCode = code,
            VehicleId = vehicle.Id,
            PickupLocationId = pickupLoc.Id,
            ReturnLocationId = returnLoc.Id,
            PickupDate = request.PickupDate,
            ReturnDate = request.ReturnDate,
            TotalDays = totalDays,
            DailyRate = vehicle.DailyPrice,
            VehicleTotal = vehicleTotal,
            ExtrasTotal = extrasTotal,
            GrandTotal = vehicleTotal + extrasTotal,
            DepositAmount = vehicle.DepositAmount,
            Status = BookingStatus.Confirmed,
            CreatedAt = DateTime.Now,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            Phone = request.Phone.Trim(),
            NationalId = request.NationalId.Trim(),
            BirthDate = request.BirthDate ?? new DateOnly(1995, 1, 1),
            LicenseIssueYear = request.LicenseIssueYear ?? 2018,
            FlightNumber = request.FlightNumber?.Trim(),
            Notes = request.Notes?.Trim(),
            HasFullInsurance = request.HasFullInsurance,
            HasAdditionalDriver = request.HasAdditionalDriver,
            HasChildSeat = request.HasChildSeat,
            HasTollPass = request.HasTollPass
        };

        context.Bookings.Add(booking);
        context.SaveChanges();

        // Load navigations
        booking.Vehicle = vehicle;
        booking.PickupLocation = pickupLoc;
        booking.ReturnLocation = returnLoc;

        return booking;
    }

    public Booking? GetBookingByCode(string bookingCode)
    {
        if (string.IsNullOrWhiteSpace(bookingCode)) return null;

        var cleanCode = bookingCode.Trim().ToLower();
        return context.Bookings
            .Include(b => b.Vehicle)
            .Include(b => b.PickupLocation)
            .Include(b => b.ReturnLocation)
            .FirstOrDefault(b => b.BookingCode.ToLower() == cleanCode);
    }

    public Booking? FindBooking(string? bookingCode, string? contactInfo)
    {
        var cleanCode = bookingCode?.Trim().ToLower();
        var cleanContact = contactInfo?.Trim().ToLower();

        var query = context.Bookings
            .Include(b => b.Vehicle)
            .Include(b => b.PickupLocation)
            .Include(b => b.ReturnLocation)
            .AsQueryable();

        if (!string.IsNullOrEmpty(cleanCode))
        {
            var matchByCode = query.FirstOrDefault(b => b.BookingCode.ToLower() == cleanCode);
            if (matchByCode != null)
            {
                if (string.IsNullOrEmpty(cleanContact) ||
                    matchByCode.Email.ToLower() == cleanContact ||
                    matchByCode.Phone.Contains(cleanContact))
                {
                    return matchByCode;
                }
            }
        }

        if (!string.IsNullOrEmpty(cleanContact))
        {
            return query.FirstOrDefault(b =>
                b.Email.ToLower() == cleanContact ||
                b.Phone.Contains(cleanContact));
        }

        return null;
    }

    public bool CancelBooking(string bookingCode)
    {
        if (string.IsNullOrWhiteSpace(bookingCode)) return false;

        var cleanCode = bookingCode.Trim().ToLower();
        var booking = context.Bookings.FirstOrDefault(b => b.BookingCode.ToLower() == cleanCode);

        if (booking != null)
        {
            booking.Status = BookingStatus.Cancelled;
            context.SaveChanges();
            return true;
        }

        return false;
    }

    public IReadOnlyList<Booking> GetAllBookings() =>
        context.Bookings
            .Include(b => b.Vehicle)
            .Include(b => b.PickupLocation)
            .Include(b => b.ReturnLocation)
            .OrderByDescending(b => b.CreatedAt)
            .ToList();
}
