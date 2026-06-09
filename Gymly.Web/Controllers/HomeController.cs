using Gymly.Application.Interfaces;
using Gymly.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Gymly.Web.Controllers;

public class HomeController(IApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var totalMembers = await context.Members.CountAsync(cancellationToken);
        var activeMembers = await context.Members.CountAsync(m => m.IsActive, cancellationToken);
        var inactiveMembers = totalMembers - activeMembers;

        var trainersCount = await context.Trainers.CountAsync(cancellationToken);

        var classesCount = await context.Classes.CountAsync(cancellationToken);

        var activeMemberships = await context.Memberships
            .CountAsync(m => m.Status == Domain.Enums.MembershipStatus.Active
                && m.EndDate >= DateTime.UtcNow, cancellationToken);

        var activePlans = await context.Plans.CountAsync(p => p.IsActive, cancellationToken);

        var todayStart = DateTime.UtcNow.Date;
        var todayEnd = todayStart.AddDays(1);
        var todaySessions = await context.Sessions
            .CountAsync(s => s.StartTime >= todayStart && s.StartTime < todayEnd, cancellationToken);

        var upcomingSessions = await context.Sessions
            .Include(s => s.Class)
            .Include(s => s.Trainer)
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

        var todayCheckIns = await context.AttendanceLogs
            .CountAsync(a => a.ScannedAt >= todayStart
                && a.ScannedAt < todayEnd
                && a.WasGranted, cancellationToken);

        var totalBookingsToday = await context.Bookings
            .CountAsync(b => b.BookedAt >= todayStart && b.BookedAt < todayEnd, cancellationToken);

        ViewBag.TotalMembers = totalMembers;
        ViewBag.ActiveMembers = activeMembers;
        ViewBag.InactiveMembers = inactiveMembers;
        ViewBag.TrainersCount = trainersCount;
        ViewBag.ClassesCount = classesCount;
        ViewBag.ActiveMemberships = activeMemberships;
        ViewBag.ActivePlans = activePlans;
        ViewBag.TodaySessions = todaySessions;
        ViewBag.UpcomingSessions = upcomingSessions;
        ViewBag.TodayCheckIns = todayCheckIns;
        ViewBag.TotalBookingsToday = totalBookingsToday;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class UpcomingSessionDto
{
    public string ClassName { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}