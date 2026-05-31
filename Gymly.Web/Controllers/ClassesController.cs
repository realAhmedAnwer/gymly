using Gymly.Application.Features.Classes.Commands.CreateClass;
using Gymly.Application.Features.Classes.Commands.DeleteClass;
using Gymly.Application.Features.Classes.Queries.GetClassesLookup;
using Gymly.Web.Models.Classes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

public class ClassesController(ISender mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var classes = await mediator.Send(new GetClassesQuery(), cancellationToken);
        return View(new ClassesIndexViewModel { Classes = classes });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateClassViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateClassViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var command = new CreateClassCommand(model.Name, model.Description, model.MaxCapacity);
            await mediator.Send(command, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Name", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteClassCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}