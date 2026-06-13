using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using Gymly.Domain.Entities;
using Gymly.Domain.Entities.Memberships;
using Gymly.Domain.Entities.Schedules;
using Gymly.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Infrastructure;

public class GymlyDbContext(DbContextOptions<GymlyDbContext> options, ICurrentUserService currentUserService) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<SystemUser> SystemUsers => Set<SystemUser>();
    public DbSet<SystemRole> SystemRoles => Set<SystemRole>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<PlanAccessRule> PlanAccessRules => Set<PlanAccessRule>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<AttendanceLog> AttendanceLogs => Set<AttendanceLog>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasSequence<int>("UserSequence", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymlyDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType, builder =>
                {
                    builder.Property(nameof(BaseEntity.CreatedAt))
                           .IsRequired();

                    builder.Property(nameof(BaseEntity.CreatedBy))
                           .IsRequired()
                           .HasMaxLength(100);

                    builder.Property(nameof(BaseEntity.UpdatedAt))
                           .IsRequired(false);

                    builder.Property(nameof(BaseEntity.UpdatedBy))
                           .IsRequired(false)
                           .HasMaxLength(100);
                    if (entityType.BaseType == null)
                    {
                        var method = typeof(GymlyDbContext)
                            .GetMethod(nameof(ConfigureGlobalSoftDeleteFilter),
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                            ?.MakeGenericMethod(entityType.ClrType);

                        method?.Invoke(null, [modelBuilder]);
                    }
                });
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUser = currentUserService.Username ?? "System";

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUser;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUser;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    private static void ConfigureGlobalSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }
}