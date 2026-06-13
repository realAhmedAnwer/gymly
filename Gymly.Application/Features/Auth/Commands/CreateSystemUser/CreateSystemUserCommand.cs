using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using Gymly.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Commands.CreateSystemUser;

public record CreateSystemUserCommand(string Username, string Email, string FullName, string Password, int SystemRoleId, string CreatedByRole) : IRequest<int>;

public class CreateSystemUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher) : IRequestHandler<CreateSystemUserCommand, int>
{
    public async Task<int> Handle(CreateSystemUserCommand request, CancellationToken cancellationToken)
    {
        if (request.CreatedByRole == "Admin")
        {
            var targetRole = await context.SystemRoles.FindAsync(request.SystemRoleId);
            if (targetRole is not null && (targetRole.Name == "Super Admin" || targetRole.Name == "Admin"))
            {
                throw new InvalidOperationException("You do not have permission to create Super Admin or Admin users.");
            }
        }

        var normalizedUsername = request.Username.Trim().ToLowerInvariant();
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        if (await context.SystemUsers.AnyAsync(u => u.Username == normalizedUsername, cancellationToken))
        {
            throw new InvalidOperationException("Username is already taken.");
        }

        if (await context.SystemUsers.AnyAsync(u => u.Email == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("Email is already in use.");
        }

        var user = new SystemUser
        {
            Username = normalizedUsername,
            Email = normalizedEmail,
            FullName = request.FullName.Trim(),
            PasswordHash = passwordHasher.Hash(request.Password),
            IsActive = true,
            SystemRoleId = request.SystemRoleId
        };

        context.SystemUsers.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
