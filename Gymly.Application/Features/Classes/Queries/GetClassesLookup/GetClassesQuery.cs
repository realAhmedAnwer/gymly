using Gymly.Application.Interfaces;
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

public class GetClassesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetClassesQuery, ClassQueryResult>
{
    public async Task<ClassQueryResult> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
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
            return new ClassQueryResult(classes, totalCount, pageNumber, request.PageSize, totalPages);
        }

        var allClasses = await query
            .OrderBy(c => c.Name)
            .Select(c => new ClassLookupDto(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        return new ClassQueryResult(allClasses, totalCount, 1, totalCount, 1);
    }
}
