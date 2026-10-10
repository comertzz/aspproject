using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspproject.Models;

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Completed
}

public class Booking
{
    [Key]
    public string BookingCode { get; set; } = string.Empty; // e.g. REZ-84920

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public int PickupLocationId { get; set; }
    public RentalLocation PickupLocation { get; set; } = null!;

    public int ReturnLocationId { get; set; }
    public RentalLocation ReturnLocation { get; set; } = null!;

    public DateOnly PickupDate { get; set; }
    public DateOnly ReturnDate { get; set; }
    public int TotalDays { get; set; }
    public decimal DailyRate { get; set; }
    public decimal VehicleTotal { get; set; }
    public decimal ExtrasTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal DepositAmount { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Driver Details
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public int LicenseIssueYear { get; set; }
    public string? FlightNumber { get; set; }
    public string? Notes { get; set; }

    // Extras
    public bool HasFullInsurance { get; set; }
    public bool HasAdditionalDriver { get; set; }
    public bool HasChildSeat { get; set; }
    public bool HasTollPass { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
