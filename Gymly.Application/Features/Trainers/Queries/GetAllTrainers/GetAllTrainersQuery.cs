using MediatR;

namespace Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

public record GetAllTrainersQuery : IRequest<List<TrainerDto>>;
