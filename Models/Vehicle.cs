namespace aspproject.Models;

public class Vehicle
{
    public int Id { get; init; }

    public required string Brand { get; init; }

    public required string Model { get; init; }

    public required string Segment { get; init; }

    public required string Transmission { get; init; }

    public required string FuelType { get; init; }

    public required string ImageUrl { get; init; }

    public IReadOnlyList<int> AvailableLocationIds { get; init; } = [];

    public int SeatCount { get; init; }

    public int BaggageCount { get; init; } = 2;

    public int DailyMileageLimit { get; init; }

    public decimal DailyPrice { get; init; }

    public decimal DepositAmount { get; init; } = 2500m;

    public int MinDriverAge { get; init; } = 21;

    public int MinLicenseYears { get; init; } = 1;

    public string FuelConsumption { get; init; } = "5.4L / 100km";

    public int Year { get; init; } = 2024;

    public bool IsFeatured { get; init; }

    public string DisplayName => $"{Brand} {Model}";
}
