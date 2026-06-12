using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Bookings.Queries.GetBookingFormData;

public record GetBookingFormDataQuery : IRequest<BookingFormDataDto>;

public class GetBookingFormDataQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBookingFormDataQuery, BookingFormDataDto>
{
    public async Task<BookingFormDataDto> Handle(GetBookingFormDataQuery request, CancellationToken cancellationToken)
    {
        var sessions = await context.Sessions
            .Include(s => s.Class)
            .Include(s => s.Bookings)
            .Where(s => s.EndTime > DateTime.UtcNow)
            .OrderBy(s => s.StartTime)
            .Select(s => new SessionOptionDto
            {
                Id = s.Id,
                Display = $"{s.Class!.Name} - {s.StartTime:yyyy-MM-dd HH:mm}",
                BookedCount = s.Bookings.Count(b => !b.IsCancelled),
                MaxCapacity = s.Class!.MaxCapacity
            })
            .ToListAsync(cancellationToken);

        return new BookingFormDataDto
        {
            AvailableSessions = sessions
        };
    }
}
