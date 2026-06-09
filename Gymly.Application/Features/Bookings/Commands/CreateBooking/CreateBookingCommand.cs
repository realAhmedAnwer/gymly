using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand(int SessionId, int MemberId) : IRequest<int>;

public class CreateBookingCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateBookingCommand, int>
{
    public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var session = await context.Sessions
            .FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);

        if (session == null)
        {
            throw new InvalidOperationException("Session not found.");
        }

        var member = await context.Members
            .FirstOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);

        if (member == null || !member.IsActive)
        {
            throw new InvalidOperationException("Member not found or inactive.");
        }

        if (session.EndTime <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Cannot book a session that has already ended.");
        }

        var activeBookingsCount = await context.Bookings
            .CountAsync(b => b.SessionId == request.SessionId && !b.IsCancelled, cancellationToken);

        var maxCapacity = await context.Classes
            .Where(c => c.Id == session.ClassId)
            .Select(c => c.MaxCapacity)
            .FirstOrDefaultAsync(cancellationToken);

        if (activeBookingsCount >= maxCapacity)
        {
            throw new InvalidOperationException("This session is at full capacity.");
        }

        var alreadyBooked = await context.Bookings
            .AnyAsync(b => b.SessionId == request.SessionId && b.MemberId == request.MemberId && !b.IsCancelled, cancellationToken);

        if (alreadyBooked)
        {
            throw new InvalidOperationException("This member is already booked for this session.");
        }

        var booking = new Domain.Entities.Schedules.Booking
        {
            SessionId = request.SessionId,
            MemberId = request.MemberId,
            BookedAt = DateTime.UtcNow,
            IsCancelled = false
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}