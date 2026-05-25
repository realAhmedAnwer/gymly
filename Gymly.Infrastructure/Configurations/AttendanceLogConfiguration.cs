using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Schedules;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Configurations;

public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
{
    public void Configure(EntityTypeBuilder<AttendanceLog> builder)
    {
        builder.ToTable("AttendanceLogs");
        builder.HasKey(al => al.Id);

        builder.Property(al => al.ScannedAt)
               .IsRequired();

        builder.Property(al => al.Method)
               .IsRequired()
               .HasConversion<string>() 
               .HasMaxLength(30);

        builder.Property(al => al.WasGranted)
               .IsRequired();

        builder.Property(al => al.RejectionReason)
               .IsRequired()
               .HasMaxLength(250);

        builder.HasOne<Member>()
               .WithMany()
               .HasForeignKey(al => al.MemberId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}