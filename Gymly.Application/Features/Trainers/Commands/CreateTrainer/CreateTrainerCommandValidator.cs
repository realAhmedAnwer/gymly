using FluentValidation;

namespace Gymly.Application.Features.Trainers.Commands.CreateTrainer;

public class CreateTrainerCommandValidator : AbstractValidator<CreateTrainerCommand>
{
    public CreateTrainerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Trainer name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Invalid email address formatting.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^0[0125]\d{9}$").WithMessage("Phone must be a valid Egyptian number (e.g. 011XXXXXXXX).");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization field is required.");
    }
}