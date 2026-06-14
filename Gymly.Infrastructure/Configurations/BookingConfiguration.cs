using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Schedules;

namespace Gymly.Infrastructure.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.IsCancelled);

        builder.HasOne(b => b.Session)
               .WithMany(s => s.Bookings)
               .HasForeignKey(b => b.SessionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Member)
               .WithMany()
               .HasForeignKey(b => b.MemberId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => new { b.SessionId, b.MemberId })
            .IsUnique()
            .HasFilter("[IsCancelled] = 0");
        builder.HasIndex(b => new { b.SessionId, b.IsCancelled });
    }
}
