namespace aspproject.Models;

public class RentalLocation
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string City { get; init; }

    public bool IsAirport { get; init; }
}
