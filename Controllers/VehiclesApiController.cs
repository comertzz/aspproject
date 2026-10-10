using aspproject.Models;
using aspproject.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspproject.Controllers;

[ApiController]
[Route("api/vehicles")]
public class VehiclesApiController : ControllerBase
{
    private readonly IVehicleCatalogService vehicleCatalogService;

    public VehiclesApiController(IVehicleCatalogService vehicleCatalogService)
    {
        this.vehicleCatalogService = vehicleCatalogService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<Vehicle>> GetVehicles([FromQuery] VehicleSearchRequest request)
    {
        return Ok(vehicleCatalogService.SearchVehicles(request));
    }

    [HttpGet("{id:int}")]
    public ActionResult<Vehicle> GetVehicle(int id)
    {
        var vehicle = vehicleCatalogService.GetVehicleById(id);

        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }
}
