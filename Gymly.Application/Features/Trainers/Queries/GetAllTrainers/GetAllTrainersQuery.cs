using Gymly.Domain.Entities.Users;
using MediatR;

namespace Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

public record GetAllTrainersQuery : IRequest<IEnumerable<Trainer>>;