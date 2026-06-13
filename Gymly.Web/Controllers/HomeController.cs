using Gymly.Application.Features.Home.Queries.GetDashboardStats;
using Gymly.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gymly.Web.Controllers;

[Authorize]
public class HomeController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var stats = await mediator.Send(new GetDashboardStatsQuery(), cancellationToken);

        ViewBag.TotalMembers = stats.TotalMembers;
        ViewBag.ActiveMembers = stats.ActiveMembers;
        ViewBag.InactiveMembers = stats.InactiveMembers;
        ViewBag.TrainersCount = stats.TrainersCount;
        ViewBag.ClassesCount = stats.ClassesCount;
        ViewBag.ActiveMemberships = stats.ActiveMemberships;
        ViewBag.ActivePlans = stats.ActivePlans;
        ViewBag.TodaySessions = stats.TodaySessions;
        ViewBag.UpcomingSessions = stats.UpcomingSessions;
        ViewBag.TodayCheckIns = stats.TodayCheckIns;
        ViewBag.TotalBookingsToday = stats.TotalBookingsToday;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
