using Gymly.Application.Features.Auth.Commands.CreateSystemUser;
using Gymly.Application.Features.Auth.Commands.DeleteSystemUser;
using Gymly.Application.Features.Auth.Commands.UpdateSystemUserProfile;
using Gymly.Application.Features.Auth.Queries.GetRolesLookup;
using Gymly.Application.Features.Auth.Queries.GetSystemUserByUsername;
using Gymly.Application.Features.Auth.Queries.GetSystemUsers;
using Gymly.Web.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

[Authorize(Roles = "Super Admin,Admin")]
public class UsersController(ISender mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new GetSystemUsersQuery(), cancellationToken);
        return View(new UsersIndexViewModel { Users = users });
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new CreateUserViewModel();
        await PopulateRolesForCurrentUser(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateRolesForCurrentUser(model, cancellationToken);
            return View(model);
        }

        try
        {
            var command = new CreateSystemUserCommand(
                model.Username, model.Email, model.FullName, model.Password, model.SystemRoleId,
                User.IsInRole("Super Admin") ? "Super Admin" : "Admin");
            await mediator.Send(command, cancellationToken);

            TempData["SuccessMessage"] = "User created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateRolesForCurrentUser(model, cancellationToken);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deletedByRole = User.IsInRole("Super Admin") ? "Super Admin" : "Admin";
            var deleted = await mediator.Send(new DeleteSystemUserCommand(id, deletedByRole), cancellationToken);
            if (deleted)
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Auth");

        var profile = await mediator.Send(new GetSystemUserByUsernameQuery(username), cancellationToken);
        if (profile is null) return RedirectToAction("Login", "Auth");

        return View(new ProfileViewModel
        {
            Id = profile.Id,
            Username = profile.Username,
            Email = profile.Email,
            FullName = profile.FullName,
            RoleName = profile.RoleName,
            IsActive = profile.IsActive
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model, CancellationToken cancellationToken)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
        {
            var profile = await mediator.Send(new GetSystemUserByUsernameQuery(username), cancellationToken);
            if (profile is not null)
            {
                model.Username = profile.Username;
                model.RoleName = profile.RoleName;
                model.IsActive = profile.IsActive;
            }
            return View(model);
        }

        try
        {
            var updated = await mediator.Send(
                new UpdateSystemUserProfileCommand(username, model.Email, model.FullName), cancellationToken);

            if (updated)
            {
                TempData["SuccessMessage"] = "Profile updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update profile.";
            }
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var profile = await mediator.Send(new GetSystemUserByUsernameQuery(username), cancellationToken);
            if (profile is not null)
            {
                model.Username = profile.Username;
                model.RoleName = profile.RoleName;
                model.IsActive = profile.IsActive;
            }
            return View(model);
        }

        return RedirectToAction(nameof(Profile));
    }

    private async Task PopulateRolesForCurrentUser(CreateUserViewModel model, CancellationToken cancellationToken)
    {
        var roles = await mediator.Send(new GetRolesLookupQuery(), cancellationToken);

        if (User.IsInRole("Admin") && !User.IsInRole("Super Admin"))
        {
            model.AvailableRoles = roles
                .Where(r => r.Name != "Super Admin" && r.Name != "Admin")
                .Select(r => new RoleOption { Id = r.Id, Name = r.Name }).ToList();
        }
        else
        {
            model.AvailableRoles = roles.Select(r => new RoleOption { Id = r.Id, Name = r.Name }).ToList();
        }
    }
}