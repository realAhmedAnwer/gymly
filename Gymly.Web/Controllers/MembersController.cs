using Gymly.Application.Features.Members.Commands.ActivateMember;
using Gymly.Application.Features.Members.Commands.CreateMember;
using Gymly.Application.Features.Members.Commands.DeactivateMember;
using Gymly.Application.Features.Members.Commands.UpdateMember;
using Gymly.Application.Features.Members.Queries.GetMemberAttendanceLogs;
using Gymly.Application.Features.Members.Queries.GetMemberById;
using Gymly.Application.Features.Members.Queries.GetMemberMembership;
using Gymly.Application.Features.Members.Queries.GetMembers;
using Gymly.Application.Interfaces.Common;
using Gymly.Web.Models.Members;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

public class MembersController(ISender mediator, IQrCodeService qrCodeService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? sortBy, bool? showInactive, string? searchTerm, int? page, CancellationToken cancellationToken)
    {
        var pageNumber = page ?? 1;
        var result = await mediator.Send(new GetMembersQuery(showInactive, sortBy, searchTerm, pageNumber, 10), cancellationToken);

        var viewModel = new MembersIndexViewModel
        {
            Members = result.Members,
            SearchTerm = searchTerm,
            SortBy = sortBy,
            ShowInactive = showInactive ?? false,
            CurrentPage = result.PageNumber,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalCount,
            PageSize = result.PageSize
        };

        ViewBag.SortBy = sortBy ?? "name";
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateMemberViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var command = new CreateMemberCommand(model.Name, model.Email, model.Phone);
            await mediator.Send(command, cancellationToken);
            TempData["SuccessMessage"] = "Member registered successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Email", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var member = await mediator.Send(new GetMemberByIdQuery(id), cancellationToken);
        if (member == null) return NotFound();

        var membership = await mediator.Send(new GetMemberMembershipQuery(id), cancellationToken);
        var attendanceLogs = await mediator.Send(new GetMemberAttendanceLogsQuery(id), cancellationToken);

        var qrPayload = member.AttendanceCardToken.ToString();
        var qrBase64 = qrCodeService.GeneratePngBase64(qrPayload);

        ViewBag.Membership = membership;
        ViewBag.AttendanceLogs = attendanceLogs;
        ViewBag.QRCodeBase64 = qrBase64;

        return View(member);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var member = await mediator.Send(new GetMemberByIdQuery(id), cancellationToken);
        if (member == null) return NotFound();

        var viewModel = new EditMemberViewModel
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            IsActive = member.IsActive,
            RegistrationDate = member.RegistrationDate
        };

        ViewBag.AttendanceCardToken = member.AttendanceCardToken;
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditMemberViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var member = await mediator.Send(new GetMemberByIdQuery(model.Id), cancellationToken);
            ViewBag.AttendanceCardToken = member?.AttendanceCardToken;
            return View(model);
        }

        try
        {
            var command = new UpdateMemberCommand(model.Id, model.Name, model.Email, model.Phone);
            var success = await mediator.Send(command, cancellationToken);
            if (!success) return NotFound();
            TempData["SuccessMessage"] = "Member updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Email", ex.Message);
            var member = await mediator.Send(new GetMemberByIdQuery(model.Id), cancellationToken);
            ViewBag.AttendanceCardToken = member?.AttendanceCardToken;
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeactivateMemberCommand(id), cancellationToken);
        TempData["SuccessMessage"] = "Member deactivated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new ActivateMemberCommand(id), cancellationToken);
        TempData["SuccessMessage"] = "Member activated.";
        return RedirectToAction(nameof(Index));
    }
}