using Gymly.Application.Features.Memberships.Commands.AssignMemberToPlan;
using Gymly.Application.Interfaces;
using Gymly.Web.Models.Memberships;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Web.Controllers;

public class MembershipsController(ISender mediator, IApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(int? memberId, CancellationToken cancellationToken)
    {
        if (memberId == null)
        {
            TempData["ErrorMessage"] = "Please select a member first.";
            return RedirectToAction("Index", "Members");
        }

        var member = await context.Members
            .FirstOrDefaultAsync(m => m.Id == memberId.Value && m.IsActive, cancellationToken);

        if (member == null)
        {
            TempData["ErrorMessage"] = "Member not found or inactive.";
            return RedirectToAction("Index", "Members");
        }

        var viewModel = new AssignMembershipViewModel
        {
            MemberId = member.Id,
            MemberName = member.Name,
            AvailablePlans = await context.Plans
                .Where(p => p.IsActive)
                .OrderBy(p => p.Title)
                .Select(p => new PlanOption
                {
                    Id = p.Id,
                    Title = p.Title,
                    DurationInDays = p.DurationInDays
                })
                .ToListAsync(cancellationToken)
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AssignMembershipViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            model.MemberName = (await context.Members
                .FirstOrDefaultAsync(m => m.Id == model.MemberId, cancellationToken))?.Name;

            model.AvailablePlans = await context.Plans
                .Where(p => p.IsActive)
                .OrderBy(p => p.Title)
                .Select(p => new PlanOption
                {
                    Id = p.Id,
                    Title = p.Title,
                    DurationInDays = p.DurationInDays
                })
                .ToListAsync(cancellationToken);

            return View(model);
        }

        try
        {
            await mediator.Send(new AssignMemberToPlanCommand(
                model.MemberId,
                model.PlanId,
                model.StartDate,
                model.DurationInDays), cancellationToken);

            TempData["SuccessMessage"] = "Membership assigned successfully.";
            return RedirectToAction("Details", "Members", new { id = model.MemberId });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            model.MemberName = (await context.Members
                .FirstOrDefaultAsync(m => m.Id == model.MemberId, cancellationToken))?.Name;

            model.AvailablePlans = await context.Plans
                .Where(p => p.IsActive)
                .OrderBy(p => p.Title)
                .Select(p => new PlanOption
                {
                    Id = p.Id,
                    Title = p.Title,
                    DurationInDays = p.DurationInDays
                })
                .ToListAsync(cancellationToken);

            return View(model);
        }
    }
}