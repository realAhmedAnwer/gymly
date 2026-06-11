using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Commands.DeleteTrainer;

public record DeleteTrainerCommand(int TrainerId) : IRequest<bool>;

public class DeleteTrainerCommandHandler(
    IApplicationDbContext context)
    : IRequestHandler<DeleteTrainerCommand, bool>
{
    public async Task<bool> Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await context.Trainers
            .FirstOrDefaultAsync(t => t.Id == request.TrainerId, cancellationToken);

        if (trainer == null) return false;

        context.Trainers.Remove(trainer);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}