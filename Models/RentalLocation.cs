namespace aspproject.Models;

public class RentalLocation
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public bool IsAirport { get; set; }
}
