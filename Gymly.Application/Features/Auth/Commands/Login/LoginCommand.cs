using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Username, string Password) : IRequest<LoginResult>;

public record LoginResult(bool Success, string? ErrorMessage, string? UserId, string? Username, string? FullName, string? Role);

public class LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedUsername = request.Username.Trim().ToLowerInvariant();
        var user = await context.SystemUsers
            .AsNoTracking()
            .Include(u => u.SystemRole)
            .FirstOrDefaultAsync(u => u.Username == normalizedUsername, cancellationToken);

        if (user is null)
        {
            return new LoginResult(false, "Invalid username or password.", null, null, null, null);
        }

        if (!user.IsActive)
        {
            return new LoginResult(false, "This account has been deactivated. Contact an administrator.", null, null, null, null);
        }

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return new LoginResult(false, "Invalid username or password.", null, null, null, null);
        }

        return new LoginResult(
            true,
            null,
            user.Id.ToString(),
            user.Username,
            user.FullName,
            user.SystemRole?.Name ?? "User"
        );
    }
}