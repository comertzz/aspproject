using aspproject.Models;
using aspproject.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspproject.Controllers;

public class BookingController : Controller
{
    private readonly IVehicleCatalogService vehicleCatalogService;
    private readonly IBookingService bookingService;

    public BookingController(IVehicleCatalogService vehicleCatalogService, IBookingService bookingService)
    {
        this.vehicleCatalogService = vehicleCatalogService;
        this.bookingService = bookingService;
    }

    [HttpGet]
    public IActionResult Checkout(
        int vehicleId,
        int? pickupLocationId,
        int? returnLocationId,
        DateOnly? pickupDate,
        DateOnly? returnDate)
    {
        var vehicle = vehicleCatalogService.GetVehicleById(vehicleId);
        if (vehicle == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var locations = vehicleCatalogService.GetLocations();
        var selectedPickupLocation = locations.FirstOrDefault(l => l.Id == (pickupLocationId ?? 1)) ?? locations.First();
        var selectedReturnLocationId = returnLocationId ?? selectedPickupLocation.Id;

        var defaultPickup = pickupDate ?? DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var defaultReturn = returnDate ?? defaultPickup.AddDays(3);

        if (defaultReturn <= defaultPickup)
        {
            defaultReturn = defaultPickup.AddDays(1);
        }

        var request = new BookingCheckoutRequest
        {
            VehicleId = vehicle.Id,
            PickupLocationId = selectedPickupLocation.Id,
            ReturnLocationId = selectedReturnLocationId,
            PickupDate = defaultPickup,
            ReturnDate = defaultReturn,
            AcceptTerms = false
        };

        var viewModel = new BookingCheckoutViewModel
        {
            Vehicle = vehicle,
            PickupLocation = selectedPickupLocation,
            Locations = locations,
            Request = request
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(BookingCheckoutRequest request)
    {
        var vehicle = vehicleCatalogService.GetVehicleById(request.VehicleId);
        if (vehicle == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var locations = vehicleCatalogService.GetLocations();
        var pickupLoc = locations.FirstOrDefault(l => l.Id == request.PickupLocationId) ?? locations.First();

        if (request.ReturnDate <= request.PickupDate)
        {
            ModelState.AddModelError(nameof(request.ReturnDate), "Dönüş tarihi, alış tarihinden sonraki bir tarih olmalıdır.");
        }

        if (request.PickupDate < DateOnly.FromDateTime(DateTime.Today))
        {
            ModelState.AddModelError(nameof(request.PickupDate), "Alış tarihi geçmiş bir tarih olamaz.");
        }

        // Driver Age validation
        if (request.BirthDate.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - request.BirthDate.Value.Year;
            if (request.BirthDate.Value > today.AddYears(-age)) age--;

            if (age < vehicle.MinDriverAge)
            {
                ModelState.AddModelError(nameof(request.BirthDate),
                    $"Bu araç için minimum sürücü yaşı {vehicle.MinDriverAge}'dir. (Mevcut yaş: {age})");
            }
        }

        // License years validation
        if (request.LicenseIssueYear.HasValue)
        {
            int currentYear = DateTime.Today.Year;
            int licenseYears = currentYear - request.LicenseIssueYear.Value;
            if (licenseYears < vehicle.MinLicenseYears)
            {
                ModelState.AddModelError(nameof(request.LicenseIssueYear),
                    $"Bu araç için minimum {vehicle.MinLicenseYears} yıllık ehliyet zorunludur.");
            }
        }

        if (!request.AcceptTerms)
        {
            ModelState.AddModelError(nameof(request.AcceptTerms), "Kiralama koşullarını onaylamanız gerekmektedir.");
            ModelState.AddModelError($"Request.{nameof(request.AcceptTerms)}", "Kiralama koşullarını onaylamanız gerekmektedir.");
        }

        if (!ModelState.IsValid)
        {
            var viewModel = new BookingCheckoutViewModel
            {
                Vehicle = vehicle,
                PickupLocation = pickupLoc,
                Locations = locations,
                Request = request
            };
            return View(viewModel);
        }

        var booking = bookingService.CreateBooking(request);

        return RedirectToAction(nameof(Confirmation), new { code = booking.BookingCode });
    }

    [HttpGet]
    public IActionResult Confirmation(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return RedirectToAction("Index", "Home");
        }

        var booking = bookingService.GetBookingByCode(code);
        if (booking == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(booking);
    }

    [HttpGet]
    public IActionResult Lookup(string? code, string? contact)
    {
        var model = new BookingLookupViewModel
        {
            BookingCode = code,
            ContactInfo = contact
        };

        if (!string.IsNullOrWhiteSpace(code) || !string.IsNullOrWhiteSpace(contact))
        {
            model.HasSearched = true;
            model.FoundBooking = bookingService.FindBooking(code, contact);

            if (model.FoundBooking == null)
            {
                model.ErrorMessage = "Girdiğiniz bilgilere ait rezervasyon kaydı bulunamadı. Lütfen PNR kodunuzu veya iletişim bilgilerinizi kontrol ediniz.";
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Lookup(BookingLookupViewModel model)
    {
        model.HasSearched = true;

        if (string.IsNullOrWhiteSpace(model.BookingCode) && string.IsNullOrWhiteSpace(model.ContactInfo))
        {
            model.ErrorMessage = "Lütfen Rezervasyon (PNR) kodu veya e-posta / telefon bilginizi giriniz.";
            return View(model);
        }

        model.FoundBooking = bookingService.FindBooking(model.BookingCode, model.ContactInfo);

        if (model.FoundBooking == null)
        {
            model.ErrorMessage = "Girdiğiniz bilgilere ait rezervasyon kaydı bulunamadı. Lütfen PNR kodunuzu veya iletişim bilgilerinizi kontrol ediniz.";
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancel(string code, string? contactInfo)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return RedirectToAction(nameof(Lookup));
        }

        var success = bookingService.CancelBooking(code);
        var model = new BookingLookupViewModel
        {
            BookingCode = code,
            ContactInfo = contactInfo,
            HasSearched = true
        };

        if (success)
        {
            model.FoundBooking = bookingService.GetBookingByCode(code);
            model.SuccessMessage = $"\"{code}\" numaralı rezervasyonunuz başarıyla iptal edildi.";
        }
        else
        {
            model.ErrorMessage = "Rezervasyon iptal edilirken bir hata oluştu veya rezervasyon bulunamadı.";
        }

        return View("Lookup", model);
    }
}
