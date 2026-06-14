using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Sessions.Queries.GetSessionsList;

public record GetSessionsListQuery(
    string? SortBy = null,
    string? SearchTerm = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
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
        var query = context.Sessions.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(s =>
                s.Class!.Name.ToLower().Contains(term) ||
                s.Trainer!.Name.ToLower().Contains(term));
        }

        if (request.DateFrom.HasValue)
        {
            query = query.Where(s => s.StartTime >= request.DateFrom.Value);
        }

        if (request.DateTo.HasValue)
        {
            var dateToExclusive = request.DateTo.Value.Date.AddDays(1);
            query = query.Where(s => s.StartTime < dateToExclusive);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = request.SortBy?.ToLowerInvariant();
        query = sortBy switch
        {
            "starttime" => query.OrderBy(s => s.StartTime),
            "classname" => query.OrderBy(s => s.Class!.Name),
            "classname_desc" => query.OrderByDescending(s => s.Class!.Name),
            "trainername" => query.OrderBy(s => s.Trainer!.Name),
            "trainername_desc" => query.OrderByDescending(s => s.Trainer!.Name),
            _ => query.OrderByDescending(s => s.StartTime)
        };

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = request.PageSize;

        var sessions = await query
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
