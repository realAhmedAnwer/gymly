using FluentValidation;

namespace Gymly.Application.Features.Sessions.Commands.CreateSession;

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
        RuleFor(x => x.ClassId)
            .GreaterThan(0).WithMessage("Please select a class.");

        RuleFor(x => x.TrainerId)
            .GreaterThan(0).WithMessage("Please assign a trainer.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");
    }
}