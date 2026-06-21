using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Classes.Queries.GetClassesLookup;

public record GetClassesQuery(
    int? PageNumber = null,
    int PageSize = 10
) : IRequest<ClassQueryResult>;

public record ClassQueryResult(
    List<ClassLookupDto> Classes,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public class GetClassesQueryHandler(IApplicationDbContext context, ICacheService cacheService)
    : IRequestHandler<GetClassesQuery, ClassQueryResult>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public async Task<ClassQueryResult> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = request.PageNumber.HasValue 
            ? CacheKeys.ClassesByPage(request.PageNumber.Value, request.PageSize) 
            : CacheKeys.AllClasses;

        var cachedResult = await cacheService.GetAsync<ClassQueryResult>(cacheKey, cancellationToken);
        if (cachedResult is not null)
            return cachedResult;

        var query = context.Classes.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        if (request.PageNumber.HasValue)
        {
            var pageNumber = Math.Max(1, request.PageNumber.Value);
            var classes = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new ClassLookupDto(c.Id, c.Name))
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            var result = new ClassQueryResult(classes, totalCount, pageNumber, request.PageSize, totalPages);
            await cacheService.SetAsync(cacheKey, result, CacheDuration, cancellationToken);
            return result;
        }

        var allClasses = await query
            .OrderBy(c => c.Name)
            .Select(c => new ClassLookupDto(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        var allResult = new ClassQueryResult(allClasses, totalCount, 1, totalCount, 1);
        await cacheService.SetAsync(CacheKeys.AllClasses, allResult, CacheDuration, cancellationToken);
        return allResult;
    }
}
