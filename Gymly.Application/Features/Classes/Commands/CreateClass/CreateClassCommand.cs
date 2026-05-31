using MediatR;

namespace Gymly.Application.Features.Classes.Commands.CreateClass;

public record CreateClassCommand(
    string Name,
    string Description,
    int MaxCapacity) : IRequest<int>;