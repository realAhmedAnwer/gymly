using Gymly.Application.Interfaces;
using Gymly.Domain.Entities.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    int ClassId,
    int TrainerId,
    DateTime StartTime,
    DateTime EndTime) : IRequest<int>;

public class CreateSessionCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateSessionCommand, int>
{
    public async Task<int> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        if (request.EndTime <= request.StartTime)
        {
            throw new ArgumentException("Session execution end time must fall after the starting mark.");
        }

        var maxCapacity = await context.Classes
            .Where(c => c.Id == request.ClassId)
            .Select(c => c.MaxCapacity)
            .FirstOrDefaultAsync(cancellationToken);

        if (maxCapacity == 0)
        {
            throw new ArgumentException("The specified fitness class does not exist.");
        }

        if (maxCapacity <= 0)
        {
            throw new ArgumentException("Cannot schedule a session for a class with zero or negative capacity.");
        }

        var trainerExists = await context.Trainers.AnyAsync(t => t.Id == request.TrainerId, cancellationToken);
        if (!trainerExists)
        {
            throw new ArgumentException("The specified trainer does not exist.");
        }

        var hasTrainerOverlap = await context.Sessions.AnyAsync(
            s => s.TrainerId == request.TrainerId
              && s.StartTime < request.EndTime
              && s.EndTime > request.StartTime,
            cancellationToken);

        if (hasTrainerOverlap)
        {
            throw new ArgumentException("The trainer has an overlapping session during the requested time slot.");
        }

        var classHasOverlap = await context.Sessions.AnyAsync(
            s => s.ClassId == request.ClassId
              && s.StartTime < request.EndTime
              && s.EndTime > request.StartTime,
            cancellationToken);

        if (classHasOverlap)
        {
            throw new ArgumentException("The class already has a scheduled session during the requested time slot.");
        }

        var session = new Session
        {
            ClassId = request.ClassId,
            TrainerId = request.TrainerId,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        context.Sessions.Add(session);
        await context.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}
