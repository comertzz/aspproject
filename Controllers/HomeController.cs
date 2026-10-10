using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspproject.Models;
using aspproject.Services;

namespace aspproject.Controllers;

public class HomeController : Controller
{
    private readonly IVehicleCatalogService vehicleCatalogService;

    public HomeController(IVehicleCatalogService vehicleCatalogService)
    {
        this.vehicleCatalogService = vehicleCatalogService;
    }

    public IActionResult Index([FromQuery] VehicleSearchRequest search)
    {
        if (search.LocationId <= 0)
        {
            search.LocationId = 1;
        }

        if (!search.PickupDate.HasValue)
        {
            search.PickupDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        }

        if (!search.ReturnDate.HasValue || search.ReturnDate <= search.PickupDate)
        {
            search.ReturnDate = search.PickupDate.Value.AddDays(3);
        }

        var viewModel = new HomeIndexViewModel
        {
            Search = search,
            Locations = vehicleCatalogService.GetLocations(),
            Vehicles = vehicleCatalogService.SearchVehicles(search),
            Segments = vehicleCatalogService.GetSegments(),
            SelectedLocation = vehicleCatalogService.GetLocationById(search.LocationId)
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
