using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Memberships;

namespace Gymly.Infrastructure.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Status).IsRequired().HasMaxLength(30);

        builder.HasOne(m => m.Member)
                       .WithMany()
                       .HasForeignKey(m => m.MemberId)
                       .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Plan)
               .WithMany()
               .HasForeignKey(m => m.PlanId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}