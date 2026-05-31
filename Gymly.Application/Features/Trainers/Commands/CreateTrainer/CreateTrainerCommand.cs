using MediatR;

namespace Gymly.Application.Features.Trainers.Commands.CreateTrainer;

public record CreateTrainerCommand(
    string Name,
    string Email,
    string Phone,
    string Specialization) : IRequest<int>;