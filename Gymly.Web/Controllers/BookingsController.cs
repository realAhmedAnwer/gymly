using Gymly.Application.Features.Bookings.Commands.CancelBooking;
using Gymly.Application.Features.Bookings.Commands.CreateBooking;
using Gymly.Application.Features.Bookings.Queries.GetBookings;
using Gymly.Application.Features.Bookings.Queries.GetBookingFormData;
using Gymly.Web.Models.Bookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymly.Web.Controllers;

public class BookingsController(ISender mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int? sessionId, int? memberId, bool? showCancelled, int? page, CancellationToken cancellationToken)
    {
        var pageNumber = page ?? 1;
        var result = await mediator.Send(new GetBookingsQuery(sessionId, memberId, showCancelled, pageNumber, 10), cancellationToken);

        var viewModel = new BookingsIndexViewModel
        {
            Bookings = result.Bookings,
            CurrentPage = result.PageNumber,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalCount,
            ShowCancelled = showCancelled,
            FilterSessionId = sessionId,
            FilterMemberId = memberId
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var formData = await mediator.Send(new GetBookingFormDataQuery(), cancellationToken);

        var viewModel = new CreateBookingViewModel
        {
            AvailableSessions = formData.AvailableSessions.Select(s => new SessionOption
            {
                Id = s.Id,
                Display = s.Display,
                BookedCount = s.BookedCount,
                MaxCapacity = s.MaxCapacity
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model, cancellationToken);
            return View(model);
        }

        try
        {
            var bookingId = await mediator.Send(new CreateBookingCommand(model.SessionId, model.MemberId), cancellationToken);
            TempData["SuccessMessage"] = "Booking created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateDropdowns(model, cancellationToken);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new CancelBookingCommand(id), cancellationToken);
        TempData["SuccessMessage"] = "Booking cancelled.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns(CreateBookingViewModel model, CancellationToken cancellationToken)
    {
        var formData = await mediator.Send(new GetBookingFormDataQuery(), cancellationToken);

        model.AvailableSessions = formData.AvailableSessions.Select(s => new SessionOption
        {
            Id = s.Id,
            Display = s.Display,
            BookedCount = s.BookedCount,
            MaxCapacity = s.MaxCapacity
        }).ToList();
    }
}