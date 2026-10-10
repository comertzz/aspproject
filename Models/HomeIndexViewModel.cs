namespace aspproject.Models;

public class HomeIndexViewModel
{
    public required VehicleSearchRequest Search { get; init; }

    public required IReadOnlyList<RentalLocation> Locations { get; init; }

    public required IReadOnlyList<Vehicle> Vehicles { get; init; }

    public required IReadOnlyList<string> Segments { get; init; }

    public RentalLocation? SelectedLocation { get; init; }
}
