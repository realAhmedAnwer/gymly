using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Commands.UpdateSystemUserProfile;

public record UpdateSystemUserProfileCommand(string Username, string Email, string FullName) : IRequest<bool>;

public class UpdateSystemUserProfileCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateSystemUserProfileCommand, bool>
{
    public async Task<bool> Handle(UpdateSystemUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await context.SystemUsers.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
        if (user is null) return false;

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (await context.SystemUsers.AnyAsync(u => u.Email == normalizedEmail && u.Id != user.Id, cancellationToken))
        {
            throw new InvalidOperationException("Email is already in use.");
        }

        user.FullName = request.FullName.Trim();
        user.Email = normalizedEmail;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}