using Gymly.Application.Features.Attendance.Commands.ProcessCheckIn;
using Gymly.Application.Features.Attendance.Queries.GetRecentCheckIns;
using Gymly.Application.Features.Members.Queries.SearchMembers;
using Gymly.Application.Interfaces;
using Gymly.Domain.Enums;
using Gymly.Web.Models.Attendance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

public class AttendanceController(ISender mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var recentCheckIns = await mediator.Send(new GetRecentCheckInsQuery(50), cancellationToken);

        var viewModel = new AttendanceIndexViewModel
        {
            RecentCheckIns = recentCheckIns,
            CheckIn = new CheckInViewModel()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckInByQr(string qrToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(qrToken))
        {
            TempData["ErrorMessage"] = "Please enter or scan a QR code.";
            return RedirectToAction(nameof(Index));
        }

        if (!Guid.TryParse(qrToken.Trim(), out var token))
        {
            TempData["ErrorMessage"] = "Invalid QR code format.";
            return RedirectToAction(nameof(Index));
        }

        var result = await mediator.Send(new ProcessCheckInCommand(token, null, AccessMethod.QRCodeScanner), cancellationToken);

        if (result.WasGranted)
        {
            TempData["SuccessMessage"] = $"Check-in granted: {result.MemberName}";
        }
        else
        {
            TempData["ErrorMessage"] = $"Check-in denied: {result.RejectionReason}";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManualCheckIn(int memberId, CancellationToken cancellationToken)
    {
        if (memberId <= 0)
        {
            TempData["ErrorMessage"] = "Please select a member.";
            return RedirectToAction(nameof(Index));
        }

        var result = await mediator.Send(new ProcessCheckInCommand(null, memberId, AccessMethod.ManualReceptionOverride), cancellationToken);

        if (result.WasGranted)
        {
            TempData["SuccessMessage"] = $"Check-in granted: {result.MemberName}";
        }
        else
        {
            TempData["ErrorMessage"] = $"Check-in denied: {result.RejectionReason}";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> SearchMembers(string term, CancellationToken cancellationToken)
    {
        var results = await mediator.Send(new SearchMembersQuery(term ?? string.Empty), cancellationToken);
        return Json(results);
    }
}
