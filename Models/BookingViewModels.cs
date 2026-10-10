using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace aspproject.Models;

public class BookingCheckoutRequest
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    public int PickupLocationId { get; set; }

    public int ReturnLocationId { get; set; }

    [Required(ErrorMessage = "Alış tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateOnly PickupDate { get; set; }

    [Required(ErrorMessage = "Dönüş tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateOnly ReturnDate { get; set; }

    [Required(ErrorMessage = "Lütfen adınızı giriniz.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad en az 2 karakter olmalıdır.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lütfen soyadınızı giriniz.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad en az 2 karakter olmalıdır.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lütfen telefon numaranızı giriniz.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "T.C. Kimlik / Pasaport No zorunludur.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Kimlik / Pasaport numarası geçerli olmalıdır.")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lütfen doğum tarihinizi giriniz.")]
    [DataType(DataType.Date)]
    public DateOnly? BirthDate { get; set; }

    [Required(ErrorMessage = "Lütfen ehliyet alış yılınızı giriniz.")]
    [Range(1950, 2026, ErrorMessage = "Geçerli bir yıl giriniz.")]
    public int? LicenseIssueYear { get; set; }

    public string? FlightNumber { get; set; }

    public string? Notes { get; set; }

    // Extras
    public bool HasFullInsurance { get; set; }
    public bool HasAdditionalDriver { get; set; }
    public bool HasChildSeat { get; set; }
    public bool HasTollPass { get; set; }

    [MustBeTrue(ErrorMessage = "Kiralama koşullarını onaylamanız gerekmektedir.")]
    public bool AcceptTerms { get; set; }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class MustBeTrueAttribute : ValidationAttribute, IClientModelValidator
{
    public MustBeTrueAttribute() : base("Kiralama koşullarını onaylamanız gerekmektedir.")
    {
    }

    public override bool IsValid(object? value)
    {
        return value is true;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-mustbetrue"] = ErrorMessageString;
    }
}

public class BookingCheckoutViewModel
{
    public required Vehicle Vehicle { get; init; }
    public required RentalLocation PickupLocation { get; init; }
    public required IReadOnlyList<RentalLocation> Locations { get; init; }
    public required BookingCheckoutRequest Request { get; set; }

    public int TotalDays => Math.Max(1, Request.ReturnDate.DayNumber - Request.PickupDate.DayNumber);
    public decimal VehicleTotal => Vehicle.DailyPrice * TotalDays;

    public decimal InsuranceDailyPrice => 300m;
    public decimal AdditionalDriverDailyPrice => 150m;
    public decimal ChildSeatDailyPrice => 120m;
    public decimal TollPassOneTimePrice => 350m;

    public decimal CalculateExtrasTotal()
    {
        decimal extras = 0m;
        if (Request.HasFullInsurance) extras += InsuranceDailyPrice * TotalDays;
        if (Request.HasAdditionalDriver) extras += AdditionalDriverDailyPrice * TotalDays;
        if (Request.HasChildSeat) extras += ChildSeatDailyPrice * TotalDays;
        if (Request.HasTollPass) extras += TollPassOneTimePrice;
        return extras;
    }

    public decimal GrandTotal => VehicleTotal + CalculateExtrasTotal();
}

public class BookingLookupViewModel
{
    [Display(Name = "Rezervasyon Kodu (PNR)")]
    public string? BookingCode { get; set; }

    [Display(Name = "E-posta veya Telefon")]
    public string? ContactInfo { get; set; }

    public Booking? FoundBooking { get; set; }
    public bool HasSearched { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
}
