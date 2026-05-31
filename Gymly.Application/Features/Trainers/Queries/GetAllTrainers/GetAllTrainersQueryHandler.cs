using Gymly.Application.Interfaces.Repositories;
using Gymly.Domain.Entities.Users;
using MediatR;

namespace Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

public class GetAllTrainersQueryHandler : IRequestHandler<GetAllTrainersQuery, IEnumerable<Trainer>>
{
    private readonly ITrainerRepository _trainerRepository;

    public GetAllTrainersQueryHandler(ITrainerRepository trainerRepository)
    {
        _trainerRepository = trainerRepository;
    }

    public async Task<IEnumerable<Trainer>> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
    {
        return await _trainerRepository.GetAllTrainersAsync(cancellationToken);
    }
}