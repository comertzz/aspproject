using aspproject.Models;

namespace aspproject.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        // 1. Locations Seed
        if (!context.Locations.Any())
        {
            var locations = new List<RentalLocation>
            {
                new() { Id = 1, Name = "İstanbul Havalimanı (IST)", City = "İstanbul", IsAirport = true },
                new() { Id = 2, Name = "Sabiha Gökçen Havalimanı (SAW)", City = "İstanbul", IsAirport = true },
                new() { Id = 3, Name = "Ankara Merkez Ofis", City = "Ankara", IsAirport = false },
                new() { Id = 4, Name = "İzmir Alsancak Ofis", City = "İzmir", IsAirport = false }
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();
        }

        // 2. Vehicles Seed
        if (!context.Vehicles.Any())
        {
            var vehicles = new List<Vehicle>
            {
                new()
                {
                    Id = 1,
                    Brand = "Renault",
                    Model = "Clio",
                    Segment = "Ekonomik",
                    Transmission = "Otomatik",
                    FuelType = "Benzin",
                    SeatCount = 5,
                    BaggageCount = 2,
                    DailyMileageLimit = 350,
                    DailyPrice = 1250,
                    DepositAmount = 2000,
                    MinDriverAge = 21,
                    MinLicenseYears = 1,
                    FuelConsumption = "5.0L / 100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 3, 4],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/de/Renault_Clio_V_IMG_4181.jpg/1280px-Renault_Clio_V_IMG_4181.jpg"
                },
                new()
                {
                    Id = 2,
                    Brand = "Fiat",
                    Model = "Egea Sedan",
                    Segment = "Ekonomik",
                    Transmission = "Manuel",
                    FuelType = "Dizel",
                    SeatCount = 5,
                    BaggageCount = 3,
                    DailyMileageLimit = 400,
                    DailyPrice = 1100,
                    DepositAmount = 2000,
                    MinDriverAge = 21,
                    MinLicenseYears = 1,
                    FuelConsumption = "4.6L / 100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 3, 4],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/10/Fiat_Tipo_Sedan_Facelift_Leonberg_2022_1X7A0412.jpg/1280px-Fiat_Tipo_Sedan_Facelift_Leonberg_2022_1X7A0412.jpg"
                },
                new()
                {
                    Id = 3,
                    Brand = "Volkswagen",
                    Model = "Golf",
                    Segment = "Kompakt",
                    Transmission = "Otomatik",
                    FuelType = "Benzin",
                    SeatCount = 5,
                    BaggageCount = 2,
                    DailyMileageLimit = 350,
                    DailyPrice = 1650,
                    DepositAmount = 2500,
                    MinDriverAge = 22,
                    MinLicenseYears = 2,
                    FuelConsumption = "5.3L / 100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 3, 4],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/70/VW_Golf_VIII_Life_1.5_TSI.jpg/1280px-VW_Golf_VIII_Life_1.5_TSI.jpg",
                    IsFeatured = true
                },
                new()
                {
                    Id = 4,
                    Brand = "Hyundai",
                    Model = "Tucson",
                    Segment = "Aile & SUV",
                    Transmission = "Otomatik",
                    FuelType = "Dizel",
                    SeatCount = 5,
                    BaggageCount = 4,
                    DailyMileageLimit = 400,
                    DailyPrice = 2100,
                    DepositAmount = 3000,
                    MinDriverAge = 23,
                    MinLicenseYears = 2,
                    FuelConsumption = "5.8L / 100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 3, 4],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f3/Hyundai_Tucson_%28NX4%29_IMG_3918.jpg/1280px-Hyundai_Tucson_%28NX4%29_IMG_3918.jpg",
                    IsFeatured = true
                },
                new()
                {
                    Id = 5,
                    Brand = "Peugeot",
                    Model = "3008 GT",
                    Segment = "Aile & SUV",
                    Transmission = "Otomatik",
                    FuelType = "Dizel",
                    SeatCount = 5,
                    BaggageCount = 4,
                    DailyMileageLimit = 400,
                    DailyPrice = 2350,
                    DepositAmount = 3000,
                    MinDriverAge = 23,
                    MinLicenseYears = 2,
                    FuelConsumption = "5.5L / 100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 4],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/58/Peugeot_3008_C_IMG_9394.jpg/1280px-Peugeot_3008_C_IMG_9394.jpg"
                },
                new()
                {
                    Id = 6,
                    Brand = "BMW",
                    Model = "3 Serisi",
                    Segment = "Premium",
                    Transmission = "Otomatik",
                    FuelType = "Benzin",
                    SeatCount = 5,
                    BaggageCount = 3,
                    DailyMileageLimit = 300,
                    DailyPrice = 3450,
                    DepositAmount = 5000,
                    MinDriverAge = 25,
                    MinLicenseYears = 3,
                    FuelConsumption = "6.4L / 100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 3, 4],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BMW_G20_LCI_320i_M_Sport_M_Portimao_Blue_Metallic_%2816%29.jpg/1280px-BMW_G20_LCI_320i_M_Sport_M_Portimao_Blue_Metallic_%2816%29.jpg",
                    IsFeatured = true
                },
                new()
                {
                    Id = 7,
                    Brand = "Mercedes-Benz",
                    Model = "C200 4MATIC",
                    Segment = "Premium",
                    Transmission = "Otomatik",
                    FuelType = "Benzin",
                    SeatCount = 5,
                    BaggageCount = 3,
                    DailyMileageLimit = 300,
                    DailyPrice = 3700,
                    DepositAmount = 5000,
                    MinDriverAge = 25,
                    MinLicenseYears = 3,
                    FuelConsumption = "6.7L / 100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 3],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3b/Mercedes-Benz_W206_C_300_4MATIC_AMG_Line_Polar_White_%285%29.jpg/1280px-Mercedes-Benz_W206_C_300_4MATIC_AMG_Line_Polar_White_%285%29.jpg"
                },
                new()
                {
                    Id = 8,
                    Brand = "Tesla",
                    Model = "Model Y Dual Motor",
                    Segment = "Elektrikli",
                    Transmission = "Otomatik",
                    FuelType = "Elektrik",
                    SeatCount = 5,
                    BaggageCount = 4,
                    DailyMileageLimit = 350,
                    DailyPrice = 2950,
                    DepositAmount = 4000,
                    MinDriverAge = 24,
                    MinLicenseYears = 2,
                    FuelConsumption = "16.8 kWh/100km",
                    Year = 2024,
                    AvailableLocationIds = [1, 2, 3, 4],
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Tesla_Model_Y_IMG_9499.jpg/1280px-Tesla_Model_Y_IMG_9499.jpg"
                }
            };

            context.Vehicles.AddRange(vehicles);
            context.SaveChanges();
        }

        // 3. Demo Booking Seed
        if (!context.Bookings.Any())
        {
            var firstVehicle = context.Vehicles.First();
            var firstLocation = context.Locations.First();
            var today = DateOnly.FromDateTime(DateTime.Today);

            var demoBooking = new Booking
            {
                BookingCode = "REZ-2026-001",
                VehicleId = firstVehicle.Id,
                PickupLocationId = firstLocation.Id,
                ReturnLocationId = firstLocation.Id,
                PickupDate = today.AddDays(2),
                ReturnDate = today.AddDays(5),
                TotalDays = 3,
                DailyRate = firstVehicle.DailyPrice,
                VehicleTotal = firstVehicle.DailyPrice * 3,
                ExtrasTotal = 900m,
                GrandTotal = (firstVehicle.DailyPrice * 3) + 900m,
                DepositAmount = firstVehicle.DepositAmount,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.Now.AddHours(-3),
                FirstName = "Ahmet",
                LastName = "Yılmaz",
                Email = "ahmet.yilmaz@example.com",
                Phone = "05321112233",
                NationalId = "12345678901",
                BirthDate = new DateOnly(1994, 5, 15),
                LicenseIssueYear = 2015,
                HasFullInsurance = true,
                HasAdditionalDriver = false,
                HasChildSeat = false,
                HasTollPass = false,
                Notes = "Sabiha Gökçen varışlı uçuş."
            };

            context.Bookings.Add(demoBooking);
            context.SaveChanges();
        }
    }
}
