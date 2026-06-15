using FluentValidation;

namespace Gymly.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0).WithMessage("Please select a session.");

        RuleFor(x => x.MemberId)
            .GreaterThan(0).WithMessage("Please select a member.");
    }
}