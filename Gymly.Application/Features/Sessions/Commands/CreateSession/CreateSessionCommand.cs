using Gymly.Application.Interfaces;
using Gymly.Domain.Entities.Schedules;
using MediatR;

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