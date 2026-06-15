using FluentValidation;

namespace Gymly.Application.Features.Classes.Commands.CreateClass;

public class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Class name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.MaxCapacity)
            .InclusiveBetween(1, 200).WithMessage("Capacity must be between 1 and 200 attendees.");
    }
}