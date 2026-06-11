using Gymly.Application.Features.Memberships.Commands.AssignMemberToPlan;
using Gymly.Application.Features.Memberships.Queries.GetMembershipFormData;
using Gymly.Web.Models.Memberships;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

public class MembershipsController(ISender mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(int? memberId, CancellationToken cancellationToken)
    {
        if (memberId == null)
        {
            TempData["ErrorMessage"] = "Please select a member first.";
            return RedirectToAction("Index", "Members");
        }

        var formData = await mediator.Send(new GetMembershipFormDataQuery(memberId.Value), cancellationToken);

        if (formData == null)
        {
            TempData["ErrorMessage"] = "Member not found or inactive.";
            return RedirectToAction("Index", "Members");
        }

        var viewModel = new AssignMembershipViewModel
        {
            MemberId = formData.MemberId,
            MemberName = formData.MemberName,
            AvailablePlans = formData.AvailablePlans.Select(p => new PlanOption
            {
                Id = p.Id,
                Title = p.Title,
                DurationInDays = p.DurationInDays
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AssignMembershipViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateFormData(model, cancellationToken);
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
            await PopulateFormData(model, cancellationToken);
            return View(model);
        }
    }

    private async Task PopulateFormData(AssignMembershipViewModel model, CancellationToken cancellationToken)
    {
        var formData = await mediator.Send(new GetMembershipFormDataQuery(model.MemberId), cancellationToken);
        if (formData != null)
        {
            model.MemberName = formData.MemberName;
            model.AvailablePlans = formData.AvailablePlans.Select(p => new PlanOption
            {
                Id = p.Id,
                Title = p.Title,
                DurationInDays = p.DurationInDays
            }).ToList();
        }
    }
}