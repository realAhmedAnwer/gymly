using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Sessions.Queries.GetSessionsList;

public record GetSessionsListQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<SessionPagedResult>;

public record SessionPagedResult(
    List<SessionDto> Sessions,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public class GetSessionsListQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSessionsListQuery, SessionPagedResult>
{
    public async Task<SessionPagedResult> Handle(GetSessionsListQuery request, CancellationToken cancellationToken)
    {
        var query = context.Sessions.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = request.PageSize;

        var sessions = await query
            .OrderByDescending(s => s.StartTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SessionDto(
                s.Id,
                s.Class!.Name,
                s.Trainer!.Name,
                s.StartTime,
                s.EndTime,
                s.Bookings.Count(b => !b.IsCancelled),
                s.Class!.MaxCapacity
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new SessionPagedResult(sessions, totalCount, pageNumber, pageSize, totalPages);
    }
}
