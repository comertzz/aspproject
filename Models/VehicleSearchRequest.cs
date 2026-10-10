using System.ComponentModel.DataAnnotations;

namespace aspproject.Models;

public class VehicleSearchRequest
{
    [Required]
    public int LocationId { get; set; } = 1;

    [DataType(DataType.Date)]
    public DateOnly? PickupDate { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? ReturnDate { get; set; }

    public string? Segment { get; set; }

    public string? Transmission { get; set; }

    public string? FuelType { get; set; }
}
