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
    public async Task<IActionResult> Index()
    {
        var sessionDtos = await mediator.Send(new GetSessionsListQuery());
        return View(new SessionsDashboardViewModel { Sessions = sessionDtos });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var classes = await mediator.Send(new GetClassesQuery());
        var trainers = await mediator.Send(new GetTrainersQuery());

        var viewModel = new CreateSessionViewModel
        {
            AvailableClasses = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList(),
            AvailableTrainers = trainers.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSessionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var classes = await mediator.Send(new GetClassesQuery());
            var trainers = await mediator.Send(new GetTrainersQuery());

            model.AvailableClasses = classes.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
            model.AvailableTrainers = trainers.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();

            return View(model);
        }

        var command = new CreateSessionCommand(model.ClassId, model.TrainerId, model.StartTime, model.EndTime);
        await mediator.Send(command);

        return RedirectToAction(nameof(Index));
    }
}