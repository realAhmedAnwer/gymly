using Gymly.Application.Features.Trainers.Commands.CreateTrainer;
using Gymly.Application.Features.Trainers.Commands.DeleteTrainer;
using Gymly.Application.Features.Trainers.Queries.GetAllTrainers;
using Gymly.Web.Models.Trainers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

public class TrainersController(IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int? page, CancellationToken cancellationToken)
    {
        var pageNumber = page ?? 1;
        var result = await mediator.Send(new GetAllTrainersQuery(pageNumber, 10), cancellationToken);

        var viewModel = new TrainersIndexViewModel
        {
            Trainers = result.Trainers,
            CurrentPage = result.PageNumber,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalCount,
            PageSize = result.PageSize
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateTrainerViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var command = new CreateTrainerCommand(model.Name, model.Email, model.Phone, model.Specialization);
            await mediator.Send(command, cancellationToken);

            TempData["SuccessMessage"] = "Trainer registered successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Email", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var success = await mediator.Send(new DeleteTrainerCommand(id), cancellationToken);

        if (success)
        {
            TempData["SuccessMessage"] = "Trainer removed successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to locate or delete the requested trainer.";
        }

        return RedirectToAction(nameof(Index));
    }
}