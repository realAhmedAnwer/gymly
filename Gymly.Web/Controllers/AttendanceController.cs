using Gymly.Application.Features.Attendance.Commands.ProcessCheckIn;
using Gymly.Application.Features.Attendance.Queries.GetRecentCheckIns;
using Gymly.Application.Interfaces;
using Gymly.Domain.Enums;
using Gymly.Web.Models.Attendance;
using Gymly.Web.Models.Bookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Web.Controllers;

public class AttendanceController(ISender mediator, IApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var recentCheckIns = await mediator.Send(new GetRecentCheckInsQuery(50), cancellationToken);

        var viewModel = new AttendanceIndexViewModel
        {
            RecentCheckIns = recentCheckIns,
            CheckIn = new CheckInViewModel
            {
                AvailableMembers = await context.Members
                    .Where(m => m.IsActive)
                    .OrderBy(m => m.Name)
                    .Select(m => new MemberOption
                    {
                        Id = m.Id,
                        Name = m.Name
                    })
                    .ToListAsync(cancellationToken)
            }
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
}