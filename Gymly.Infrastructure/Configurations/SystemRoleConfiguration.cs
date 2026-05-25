using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Configurations;

public class SystemRoleConfiguration : IEntityTypeConfiguration<SystemRole>
{
    public void Configure(EntityTypeBuilder<SystemRole> builder)
    {
        builder.ToTable("SystemRoles");
        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.Name).IsRequired().HasMaxLength(50);
        builder.Property(sr => sr.Description).IsRequired(false).HasMaxLength(250);
    }
}