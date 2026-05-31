using Gymly.Application.Interfaces.Repositories;
using MediatR;

namespace Gymly.Application.Features.Trainers.Commands.DeleteTrainer;

public class DeleteTrainerCommandHandler : IRequestHandler<DeleteTrainerCommand, bool>
{
    private readonly ITrainerRepository _trainerRepository;

    public DeleteTrainerCommandHandler(ITrainerRepository trainerRepository)
    {
        _trainerRepository = trainerRepository;
    }

    public async Task<bool> Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _trainerRepository.GetByIdAsync(request.TrainerId, cancellationToken);
        if (trainer == null) return false;
        _trainerRepository.Delete(trainer);
        return true;
    }
}