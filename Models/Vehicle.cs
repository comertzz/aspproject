using System.ComponentModel.DataAnnotations.Schema;

namespace aspproject.Models;

public class Vehicle
{
    public int Id { get; set; }

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Segment { get; set; } = string.Empty;

    public string Transmission { get; set; } = string.Empty;

    public string FuelType { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public IReadOnlyList<int> AvailableLocationIds { get; set; } = [];

    public int SeatCount { get; set; }

    public int BaggageCount { get; set; } = 2;

    public int DailyMileageLimit { get; set; }

    public decimal DailyPrice { get; set; }

    public decimal DepositAmount { get; set; } = 2500m;

    public int MinDriverAge { get; set; } = 21;

    public int MinLicenseYears { get; set; } = 1;

    public string FuelConsumption { get; set; } = "5.4L / 100km";

    public int Year { get; set; } = 2024;

    public bool IsFeatured { get; set; }

    [NotMapped]
    public string DisplayName => $"{Brand} {Model}";
}
