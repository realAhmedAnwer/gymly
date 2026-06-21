using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Commands.DeleteTrainer;

public record DeleteTrainerCommand(int TrainerId) : IRequest<bool>;

public class DeleteTrainerCommandHandler(
    IApplicationDbContext context,
    ICacheService cacheService)
    : IRequestHandler<DeleteTrainerCommand, bool>
{
    public async Task<bool> Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await context.Trainers
            .FirstOrDefaultAsync(t => t.Id == request.TrainerId, cancellationToken);

        if (trainer == null) return false;

        context.Trainers.Remove(trainer);
        await context.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveAsync(CacheKeys.AllTrainers, cancellationToken);

        return true;
    }
}
