using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Home.Queries.GetDashboardStats;

public record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;

public class GetDashboardStatsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var totalMembers = await context.Members.CountAsync(cancellationToken);
        var activeMembers = await context.Members.CountAsync(m => m.IsActive, cancellationToken);

        var todayStart = DateTime.UtcNow.Date;
        var todayEnd = todayStart.AddDays(1);

        var upcomingSessions = await context.Sessions
            .Where(s => s.StartTime > DateTime.UtcNow)
            .OrderBy(s => s.StartTime)
            .Take(5)
            .Select(s => new UpcomingSessionDto
            {
                ClassName = s.Class!.Name,
                TrainerName = s.Trainer!.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            })
            .ToListAsync(cancellationToken);

        return new DashboardStatsDto
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            InactiveMembers = totalMembers - activeMembers,
            TrainersCount = await context.Trainers.CountAsync(cancellationToken),
            ClassesCount = await context.Classes.CountAsync(cancellationToken),
            ActiveMemberships = await context.Memberships
                .CountAsync(m => m.Status == Domain.Enums.MembershipStatus.Active
                    && m.EndDate >= DateTime.UtcNow, cancellationToken),
            ActivePlans = await context.Plans.CountAsync(p => p.IsActive, cancellationToken),
            TodaySessions = await context.Sessions
                .CountAsync(s => s.StartTime >= todayStart && s.StartTime < todayEnd, cancellationToken),
            TodayCheckIns = await context.AttendanceLogs
                .CountAsync(a => a.ScannedAt >= todayStart && a.ScannedAt < todayEnd && a.WasGranted, cancellationToken),
            TotalBookingsToday = await context.Bookings
                .CountAsync(b => b.BookedAt >= todayStart && b.BookedAt < todayEnd, cancellationToken),
            UpcomingSessions = upcomingSessions
        };
    }
}