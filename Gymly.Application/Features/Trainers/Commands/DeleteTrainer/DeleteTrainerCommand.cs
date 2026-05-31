using MediatR;

namespace Gymly.Application.Features.Trainers.Commands.DeleteTrainer;

public record DeleteTrainerCommand(int TrainerId) : IRequest<bool>;