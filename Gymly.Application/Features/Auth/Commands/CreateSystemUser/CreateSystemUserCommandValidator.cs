using FluentValidation;

namespace Gymly.Application.Features.Auth.Commands.CreateSystemUser;

public class CreateSystemUserCommandValidator : AbstractValidator<CreateSystemUserCommand>
{
    public CreateSystemUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.SystemRoleId)
            .GreaterThan(0).WithMessage("Role is required.");
    }
}