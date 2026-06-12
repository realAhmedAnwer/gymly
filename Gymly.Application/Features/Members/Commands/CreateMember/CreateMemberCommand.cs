using Gymly.Application.Interfaces;
using Gymly.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Commands.CreateMember;

public record CreateMemberCommand(
    string Name,
    string Email,
    string Phone) : IRequest<int>;

public class CreateMemberCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateMemberCommand, int>
{
    public async Task<int> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailExists = await context.Members
            .AnyAsync(m => m.Email == normalizedEmail, cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException("A member with this email address already exists.");
        }

        var member = new Member
        {
            Name = request.Name,
            Email = normalizedEmail,
            Phone = request.Phone,
            RegistrationDate = DateTime.UtcNow,
            AttendanceCardToken = Guid.NewGuid(),
            IsActive = true
        };

        context.Members.Add(member);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("A member with this email address already exists.");
        }

        return member.Id;
    }
}