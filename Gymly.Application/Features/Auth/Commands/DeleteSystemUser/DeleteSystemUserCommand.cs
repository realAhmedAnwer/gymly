using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Commands.DeleteSystemUser;

public record DeleteSystemUserCommand(int Id, string DeletedByRole) : IRequest<bool>;

public class DeleteSystemUserCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteSystemUserCommand, bool>
{
    public async Task<bool> Handle(DeleteSystemUserCommand request, CancellationToken cancellationToken)
    {
        if (request.DeletedByRole == "Admin")
        {
            var user = await context.SystemUsers.FindAsync(request.Id);
            if (user is not null)
            {
                var role = await context.SystemRoles.FindAsync(user.SystemRoleId);
                if (role is not null && (role.Name == "Super Admin" || role.Name == "Admin"))
                {
                    throw new InvalidOperationException("You cannot delete Super Admin or Admin users.");
                }
            }
        }

        var userToDelete = await context.SystemUsers.FindAsync(request.Id);
        if (userToDelete is null) return false;

        context.SystemUsers.Remove(userToDelete);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}