namespace aspproject.Models;

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Completed
}

public class Booking
{
    public required string BookingCode { get; init; } // e.g. REZ-84920
    public required Vehicle Vehicle { get; init; }
    public required RentalLocation PickupLocation { get; init; }
    public required RentalLocation ReturnLocation { get; init; }
    public DateOnly PickupDate { get; init; }
    public DateOnly ReturnDate { get; init; }
    public int TotalDays { get; init; }
    public decimal DailyRate { get; init; }
    public decimal VehicleTotal { get; init; }
    public decimal ExtrasTotal { get; init; }
    public decimal GrandTotal { get; init; }
    public decimal DepositAmount { get; init; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; init; } = DateTime.Now;

    // Driver Details
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required string NationalId { get; init; }
    public DateOnly BirthDate { get; init; }
    public int LicenseIssueYear { get; init; }
    public string? FlightNumber { get; init; }
    public string? Notes { get; init; }

    // Extras
    public bool HasFullInsurance { get; init; }
    public bool HasAdditionalDriver { get; init; }
    public bool HasChildSeat { get; init; }
    public bool HasTollPass { get; init; }

    public string FullName => $"{FirstName} {LastName}";
}
