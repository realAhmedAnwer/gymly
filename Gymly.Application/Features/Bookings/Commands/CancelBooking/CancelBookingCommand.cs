using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Bookings.Commands.CancelBooking;

public record CancelBookingCommand(int BookingId) : IRequest<bool>;

public class CancelBookingCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CancelBookingCommand, bool>
{
    public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await context.Bookings
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking == null) return false;

        if (booking.IsCancelled) return true;

        booking.IsCancelled = true;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}