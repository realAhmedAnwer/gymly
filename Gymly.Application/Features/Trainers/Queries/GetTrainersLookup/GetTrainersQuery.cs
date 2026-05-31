using MediatR;

namespace Gymly.Application.Features.Trainers.Queries.GetTrainersLookup;

public record GetTrainersQuery : IRequest<IEnumerable<TrainerLookupDto>>;