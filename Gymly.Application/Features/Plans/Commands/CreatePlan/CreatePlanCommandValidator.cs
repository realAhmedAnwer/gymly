using FluentValidation;

namespace Gymly.Application.Features.Plans.Commands.CreatePlan;

public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Plan title is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Price)
            .InclusiveBetween(0.01m, 99999.99m).WithMessage("Price must be between 0.01 and 99,999.99.");

        RuleFor(x => x.DurationInDays)
            .InclusiveBetween(1, 365).WithMessage("Duration must be between 1 and 365 days.");
    }
}