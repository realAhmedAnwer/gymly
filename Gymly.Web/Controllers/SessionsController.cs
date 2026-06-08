using Gymly.Application.Features.Classes.Queries.GetClassesLookup;
using Gymly.Application.Features.Sessions.Commands.CreateSession;
using Gymly.Application.Features.Sessions.Queries.GetSessionsList;
using Gymly.Application.Features.Trainers.Queries.GetTrainersLookup;
using Gymly.Web.Models.Sessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gymly.Web.Controllers;

public class SessionsController(ISender mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var sessionDtos = await mediator.Send(new GetSessionsListQuery(), cancellationToken);
        return View(new SessionsDashboardViewModel { Sessions = sessionDtos });
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var classes = await mediator.Send(new GetClassesQuery(), cancellationToken);
        var trainers = await mediator.Send(new GetTrainersQuery(), cancellationToken);

        var viewModel = new CreateSessionViewModel
        {
            AvailableClasses = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList(),
            AvailableTrainers = trainers.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var classes = await mediator.Send(new GetClassesQuery(), cancellationToken);
            var trainers = await mediator.Send(new GetTrainersQuery(), cancellationToken);

            model.AvailableClasses = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
            model.AvailableTrainers = trainers.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();

            return View(model);
        }

        try
        {
            var command = new CreateSessionCommand(model.ClassId, model.TrainerId, model.StartTime, model.EndTime);
            await mediator.Send(command, cancellationToken);

            TempData["SuccessMessage"] = "Session scheduled successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            var classes = await mediator.Send(new GetClassesQuery(), cancellationToken);
            var trainers = await mediator.Send(new GetTrainersQuery(), cancellationToken);

            model.AvailableClasses = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
            model.AvailableTrainers = trainers.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();

            return View(model);
        }
    }
}