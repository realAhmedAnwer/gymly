using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("Trainers");

        builder.Property(t => t.Id)
               .HasDefaultValueSql("NEXT VALUE FOR dbo.UserSequence");

        builder.Property(t => t.Specialization).IsRequired(false).HasMaxLength(100);
        builder.Property(t => t.HireDate)
            .IsRequired();

        builder.HasIndex(t => t.Email).IsUnique().HasDatabaseName("IX_Trainers_Email");
    }
}