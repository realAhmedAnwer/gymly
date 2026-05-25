using Gymly.Domain.Entities.Memberships;
using Gymly.Domain.Entities.Schedules;
using Gymly.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Member> Members { get; }
    DbSet<Trainer> Trainers { get; }
    DbSet<SystemUser> SystemUsers { get; }
    DbSet<SystemRole> SystemRoles { get; }
    DbSet<Plan> Plans { get; }
    DbSet<PlanAccessRule> PlanAccessRules { get; }
    DbSet<Membership> Memberships { get; }
    DbSet<Class> Classes { get; }
    DbSet<Session> Sessions { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<AttendanceLog> AttendanceLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}