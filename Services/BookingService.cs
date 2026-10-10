using System.Collections.Concurrent;
using aspproject.Models;

namespace aspproject.Services;

public class BookingService : IBookingService
{
    private static readonly ConcurrentDictionary<string, Booking> Bookings = new(StringComparer.OrdinalIgnoreCase);
    private readonly IVehicleCatalogService vehicleCatalogService;

    public BookingService(IVehicleCatalogService vehicleCatalogService)
    {
        this.vehicleCatalogService = vehicleCatalogService;
        SeedDemoBooking();
    }

    private void SeedDemoBooking()
    {
        if (Bookings.ContainsKey("REZ-2026-001"))
        {
            return;
        }

        var demoVehicle = vehicleCatalogService.GetVehicleById(1);
        var demoLocation = vehicleCatalogService.GetLocationById(1);

        if (demoVehicle != null && demoLocation != null)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var booking = new Booking
            {
                BookingCode = "REZ-2026-001",
                Vehicle = demoVehicle,
                PickupLocation = demoLocation,
                ReturnLocation = demoLocation,
                PickupDate = today.AddDays(2),
                ReturnDate = today.AddDays(5),
                TotalDays = 3,
                DailyRate = demoVehicle.DailyPrice,
                VehicleTotal = demoVehicle.DailyPrice * 3,
                ExtrasTotal = 900m,
                GrandTotal = (demoVehicle.DailyPrice * 3) + 900m,
                DepositAmount = demoVehicle.DepositAmount,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.Now.AddHours(-3),
                FirstName = "Ahmet",
                LastName = "Yılmaz",
                Email = "ahmet.yilmaz@example.com",
                Phone = "05321112233",
                NationalId = "12345678901",
                BirthDate = new DateOnly(1994, 5, 15),
                LicenseIssueYear = 2015,
                HasFullInsurance = true,
                HasAdditionalDriver = false,
                HasChildSeat = false,
                HasTollPass = false,
                Notes = "Sabiha Gökçen varışlı uçuş."
            };

            Bookings.TryAdd(booking.BookingCode, booking);
        }
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
        } while (Bookings.ContainsKey(code));

        var booking = new Booking
        {
            BookingCode = code,
            Vehicle = vehicle,
            PickupLocation = pickupLoc,
            ReturnLocation = returnLoc,
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

        Bookings[code] = booking;
        return booking;
    }

    public Booking? GetBookingByCode(string bookingCode)
    {
        if (string.IsNullOrWhiteSpace(bookingCode)) return null;
        Bookings.TryGetValue(bookingCode.Trim(), out var booking);
        return booking;
    }

    public Booking? FindBooking(string? bookingCode, string? contactInfo)
    {
        var cleanCode = bookingCode?.Trim();
        var cleanContact = contactInfo?.Trim().ToLowerInvariant();

        if (!string.IsNullOrEmpty(cleanCode))
        {
            if (Bookings.TryGetValue(cleanCode, out var matchByCode))
            {
                if (string.IsNullOrEmpty(cleanContact) ||
                    matchByCode.Email.Equals(cleanContact, StringComparison.OrdinalIgnoreCase) ||
                    matchByCode.Phone.Contains(cleanContact, StringComparison.OrdinalIgnoreCase))
                {
                    return matchByCode;
                }
            }
        }

        if (!string.IsNullOrEmpty(cleanContact))
        {
            return Bookings.Values.FirstOrDefault(b =>
                b.Email.Equals(cleanContact, StringComparison.OrdinalIgnoreCase) ||
                b.Phone.Replace(" ", "").Contains(cleanContact.Replace(" ", ""), StringComparison.OrdinalIgnoreCase));
        }

        return null;
    }

    public bool CancelBooking(string bookingCode)
    {
        if (string.IsNullOrWhiteSpace(bookingCode)) return false;

        if (Bookings.TryGetValue(bookingCode.Trim(), out var booking))
        {
            booking.Status = BookingStatus.Cancelled;
            return true;
        }

        return false;
    }

    public IReadOnlyList<Booking> GetAllBookings() =>
        Bookings.Values.OrderByDescending(b => b.CreatedAt).ToList();
}
