using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Memberships;

namespace Gymly.Infrastructure.Configurations;

public class PlanAccessRuleConfiguration : IEntityTypeConfiguration<PlanAccessRule>
{
    public void Configure(EntityTypeBuilder<PlanAccessRule> builder)
    {
        builder.ToTable("PlanAccessRules");
        builder.HasKey(par => par.Id);

        builder.Property(par => par.RuleType).IsRequired().HasMaxLength(50);
        builder.Property(par => par.RuleValue).IsRequired().HasMaxLength(100);

        builder.HasOne<Plan>()
               .WithMany(p => p.AccessRules)
               .HasForeignKey(par => par.PlanId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}