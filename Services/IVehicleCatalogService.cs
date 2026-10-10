using aspproject.Models;

namespace aspproject.Services;

public interface IVehicleCatalogService
{
    IReadOnlyList<RentalLocation> GetLocations();

    IReadOnlyList<string> GetSegments();

    RentalLocation? GetLocationById(int id);

    IReadOnlyList<Vehicle> SearchVehicles(VehicleSearchRequest request);

    Vehicle? GetVehicleById(int id);
}
