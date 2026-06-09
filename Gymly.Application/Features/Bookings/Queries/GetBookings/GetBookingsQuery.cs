using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Bookings.Queries.GetBookings;

public record GetBookingsQuery(
    int? SessionId = null,
    int? MemberId = null,
    bool? ShowCancelled = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<BookingPagedResult>;

public record BookingPagedResult(
    List<BookingDto> Bookings,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public class GetBookingsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBookingsQuery, BookingPagedResult>
{
    public async Task<BookingPagedResult> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Bookings.AsNoTracking().AsQueryable();

        if (request.SessionId.HasValue)
        {
            query = query.Where(b => b.SessionId == request.SessionId.Value);
        }

        if (request.MemberId.HasValue)
        {
            query = query.Where(b => b.MemberId == request.MemberId.Value);
        }

        if (request.ShowCancelled != true)
        {
            query = query.Where(b => !b.IsCancelled);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = request.PageSize;

        var bookings = await query
            .OrderByDescending(b => b.BookedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookingDto(
                b.Id,
                b.SessionId,
                b.Session!.Class!.Name,
                b.Session.TrainerId,
                b.Session.Trainer.Name,
                b.MemberId,
                b.Member.Name,
                b.Session.StartTime,
                b.Session.EndTime,
                b.BookedAt,
                b.IsCancelled
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new BookingPagedResult(bookings, totalCount, pageNumber, pageSize, totalPages);
    }
}