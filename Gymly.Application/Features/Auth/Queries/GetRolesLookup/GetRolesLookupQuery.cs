using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Auth.Queries.GetRolesLookup;

public record RolesLookupDto(int Id, string Name);

public record GetRolesLookupQuery : IRequest<List<RolesLookupDto>>;

public class GetRolesLookupQueryHandler(IApplicationDbContext context, ICacheService cacheService) : IRequestHandler<GetRolesLookupQuery, List<RolesLookupDto>>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public async Task<List<RolesLookupDto>> Handle(GetRolesLookupQuery request, CancellationToken cancellationToken)
    {
        var cachedResult = await cacheService.GetAsync<List<RolesLookupDto>>(CacheKeys.AllRoles, cancellationToken);
        if (cachedResult is not null)
            return cachedResult;

        var result = await context.SystemRoles
            .AsNoTracking()
            .Where(r => r.IsActive)
            .Select(r => new RolesLookupDto(r.Id, r.Name))
            .ToListAsync(cancellationToken);

        await cacheService.SetAsync(CacheKeys.AllRoles, result, CacheDuration, cancellationToken);
        return result;
    }
}
