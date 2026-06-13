using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Queries.GetSystemUserByUsername;

public record SystemUserProfileDto(int Id, string Username, string Email, string FullName, string RoleName, bool IsActive);

public record GetSystemUserByUsernameQuery(string Username) : IRequest<SystemUserProfileDto?>;

public class GetSystemUserByUsernameQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSystemUserByUsernameQuery, SystemUserProfileDto?>
{
    public async Task<SystemUserProfileDto?> Handle(GetSystemUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        return await context.SystemUsers
            .AsNoTracking()
            .Include(u => u.SystemRole)
            .Where(u => u.Username == request.Username)
            .Select(u => new SystemUserProfileDto(
                u.Id,
                u.Username,
                u.Email,
                u.FullName,
                u.SystemRole!.Name,
                u.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}