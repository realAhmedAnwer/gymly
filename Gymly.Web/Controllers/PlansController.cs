using Gymly.Application.Features.Plans.Commands.ActivatePlan;
using Gymly.Application.Features.Plans.Commands.AddPlanAccessRule;
using Gymly.Application.Features.Plans.Commands.CreatePlan;
using Gymly.Application.Features.Plans.Commands.DeletePlan;
using Gymly.Application.Features.Plans.Commands.RemovePlanAccessRule;
using Gymly.Application.Features.Plans.Commands.UpdatePlan;
using Gymly.Application.Features.Plans.Queries.GetPlanWithRules;
using Gymly.Application.Features.Plans.Queries.GetPlans;
using Gymly.Web.Models.Plans;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

public class PlansController(ISender mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? sortBy, bool? showInactive, CancellationToken cancellationToken)
    {
        var plans = await mediator.Send(new GetPlansQuery(showInactive, sortBy), cancellationToken);
        ViewBag.SortBy = sortBy ?? "title";
        ViewBag.ShowInactive = showInactive ?? false;
        return View(new PlansIndexViewModel { Plans = plans });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreatePlanViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePlanViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var command = new CreatePlanCommand(model.Title, model.Description, model.Price, model.DurationInDays);
            await mediator.Send(command, cancellationToken);
            TempData["SuccessMessage"] = "Plan created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Title", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var plan = await mediator.Send(new GetPlanWithRulesQuery(id), cancellationToken);
        if (plan == null) return NotFound();
        return View(plan);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var plan = await mediator.Send(new GetPlanWithRulesQuery(id), cancellationToken);
        if (plan == null) return NotFound();

        var viewModel = new EditPlanViewModel
        {
            Id = plan.Id,
            Title = plan.Title,
            Description = plan.Description,
            Price = plan.Price,
            DurationInDays = plan.DurationInDays,
            IsActive = plan.IsActive,
            AccessRules = plan.AccessRules
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditPlanViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var rules = (await mediator.Send(new GetPlanWithRulesQuery(model.Id), cancellationToken))?.AccessRules ?? [];
            model.AccessRules = rules;
            return View(model);
        }

        try
        {
            var command = new UpdatePlanCommand(model.Id, model.Title, model.Description, model.Price, model.DurationInDays);
            var success = await mediator.Send(command, cancellationToken);
            if (!success) return NotFound();
            TempData["SuccessMessage"] = "Plan updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Title", ex.Message);
            var rules = (await mediator.Send(new GetPlanWithRulesQuery(model.Id), cancellationToken))?.AccessRules ?? [];
            model.AccessRules = rules;
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRule(EditPlanViewModel model, CancellationToken cancellationToken)
    {
        if (model.NewRuleType == null || string.IsNullOrWhiteSpace(model.NewRuleValue))
        {
            TempData["ErrorMessage"] = "Rule type and value are required.";
            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }

        try
        {
            await mediator.Send(new AddPlanAccessRuleCommand(model.Id, model.NewRuleType.Value, model.NewRuleValue), cancellationToken);
            TempData["SuccessMessage"] = "Access rule added.";
        }
        catch (ArgumentException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Edit), new { id = model.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRule(int ruleId, int planId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemovePlanAccessRuleCommand(ruleId), cancellationToken);
        TempData["SuccessMessage"] = "Access rule removed.";
        return RedirectToAction(nameof(Edit), new { id = planId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeletePlanCommand(id), cancellationToken);
        TempData["SuccessMessage"] = "Plan deactivated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new ActivatePlanCommand(id), cancellationToken);
        TempData["SuccessMessage"] = "Plan activated.";
        return RedirectToAction(nameof(Index));
    }
}