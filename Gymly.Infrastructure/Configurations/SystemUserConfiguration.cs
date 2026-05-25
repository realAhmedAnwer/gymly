using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Configurations;

public class SystemUserConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.ToTable("SystemUsers");
        builder.HasKey(su => su.Id);

        builder.Property(su => su.Username).IsRequired().HasMaxLength(50);
        builder.Property(su => su.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(su => su.Email).IsRequired().HasMaxLength(150);

        builder.HasIndex(su => su.Username).IsUnique();
    }
}