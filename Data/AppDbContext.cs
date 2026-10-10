using aspproject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace aspproject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<RentalLocation> Locations => Set<RentalLocation>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Vehicle Configuration
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.DailyPrice).HasPrecision(18, 2);
            entity.Property(v => v.DepositAmount).HasPrecision(18, 2);

            // Storing AvailableLocationIds as comma-separated integers
            entity.Property(v => v.AvailableLocationIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => string.IsNullOrWhiteSpace(v)
                        ? new List<int>()
                        : v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                )
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyList<int>>(
                    (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));
        });

        // RentalLocation Configuration
        modelBuilder.Entity<RentalLocation>(entity =>
        {
            entity.HasKey(l => l.Id);
        });

        // Booking Configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.BookingCode);
            entity.Property(b => b.DailyRate).HasPrecision(18, 2);
            entity.Property(b => b.VehicleTotal).HasPrecision(18, 2);
            entity.Property(b => b.ExtrasTotal).HasPrecision(18, 2);
            entity.Property(b => b.GrandTotal).HasPrecision(18, 2);
            entity.Property(b => b.DepositAmount).HasPrecision(18, 2);

            entity.HasOne(b => b.Vehicle)
                .WithMany()
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.PickupLocation)
                .WithMany()
                .HasForeignKey(b => b.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.ReturnLocation)
                .WithMany()
                .HasForeignKey(b => b.ReturnLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
