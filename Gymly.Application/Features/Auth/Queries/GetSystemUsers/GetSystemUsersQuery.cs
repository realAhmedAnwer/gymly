using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Queries.GetSystemUsers;

public record GetSystemUsersQuery : IRequest<List<SystemUserDto>>;

public class GetSystemUsersQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSystemUsersQuery, List<SystemUserDto>>
{
    public async Task<List<SystemUserDto>> Handle(GetSystemUsersQuery request, CancellationToken cancellationToken)
    {
        return await context.SystemUsers
            .AsNoTracking()
            .Include(u => u.SystemRole)
            .Select(u => new SystemUserDto(
                u.Id,
                u.Username,
                u.Email,
                u.FullName,
                u.IsActive,
                u.SystemRole!.Name))
            .ToListAsync(cancellationToken);
    }
}