using aspproject.Data;
using aspproject.Models;
using Microsoft.EntityFrameworkCore;

namespace aspproject.Services;

public class VehicleCatalogService : IVehicleCatalogService
{
    private readonly AppDbContext context;

    public VehicleCatalogService(AppDbContext context)
    {
        this.context = context;
    }

    public IReadOnlyList<RentalLocation> GetLocations() =>
        context.Locations.AsNoTracking().OrderBy(l => l.Id).ToList();

    public IReadOnlyList<string> GetSegments() =>
        context.Vehicles.AsNoTracking()
            .Select(v => v.Segment)
            .Distinct()
            .OrderBy(s => s)
            .ToList();

    public RentalLocation? GetLocationById(int id) =>
        context.Locations.AsNoTracking().FirstOrDefault(l => l.Id == id);

    public IReadOnlyList<Vehicle> SearchVehicles(VehicleSearchRequest request)
    {
        var query = context.Vehicles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Segment))
        {
            query = query.Where(v => v.Segment == request.Segment);
        }

        if (!string.IsNullOrWhiteSpace(request.Transmission))
        {
            query = query.Where(v => v.Transmission == request.Transmission);
        }

        if (!string.IsNullOrWhiteSpace(request.FuelType))
        {
            query = query.Where(v => v.FuelType == request.FuelType);
        }

        var list = query
            .OrderByDescending(v => v.IsFeatured)
            .ThenBy(v => v.DailyPrice)
            .ToList();

        return list
            .Where(v => v.AvailableLocationIds.Contains(request.LocationId))
            .ToList();
    }

    public Vehicle? GetVehicleById(int id) =>
        context.Vehicles.AsNoTracking().FirstOrDefault(v => v.Id == id);
}
