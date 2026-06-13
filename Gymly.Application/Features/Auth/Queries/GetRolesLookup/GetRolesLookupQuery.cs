using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Queries.GetRolesLookup;

public record RolesLookupDto(int Id, string Name);

public record GetRolesLookupQuery : IRequest<List<RolesLookupDto>>;

public class GetRolesLookupQueryHandler(IApplicationDbContext context) : IRequestHandler<GetRolesLookupQuery, List<RolesLookupDto>>
{
    public async Task<List<RolesLookupDto>> Handle(GetRolesLookupQuery request, CancellationToken cancellationToken)
    {
        return await context.SystemRoles
            .AsNoTracking()
            .Where(r => r.IsActive)
            .Select(r => new RolesLookupDto(r.Id, r.Name))
            .ToListAsync(cancellationToken);
    }
}