using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.Property(m => m.Id)
               .HasDefaultValueSql("NEXT VALUE FOR dbo.UserSequence");

        builder.Property(m => m.AttendanceCardToken).IsRequired();

        builder.HasIndex(m => m.AttendanceCardToken).IsUnique();
    }
}