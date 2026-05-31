using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Repositories;
using MediatR;

namespace Gymly.Application.Features.Trainers.Commands.DeleteTrainer;

public class DeleteTrainerCommandHandler(
    ITrainerRepository trainerRepository,
    IApplicationDbContext context)
    : IRequestHandler<DeleteTrainerCommand, bool>
{
    public async Task<bool> Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await trainerRepository.GetByIdAsync(request.TrainerId, cancellationToken);
        if (trainer == null) return false;
        trainerRepository.Delete(trainer);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}