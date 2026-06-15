using FluentValidation;

namespace Gymly.Application.Features.Members.Commands.CreateMember;

public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Member name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.");

        RuleFor(x => x.Phone)
            .Matches(@"^$|^0[0125]\d{9}$").WithMessage("Phone must be a valid Egyptian number (e.g. 011XXXXXXXX).");
    }
}