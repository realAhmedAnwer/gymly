using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Schedules;

namespace Gymly.Infrastructure.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();

        builder.HasOne(s => s.Class)
                       .WithMany()
                       .HasForeignKey(s => s.ClassId)
                       .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Trainer)
               .WithMany()
               .HasForeignKey(s => s.TrainerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}