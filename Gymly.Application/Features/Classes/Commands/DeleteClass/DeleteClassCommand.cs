using MediatR;

namespace Gymly.Application.Features.Classes.Commands.DeleteClass;

public record DeleteClassCommand(int ClassId) : IRequest<bool>;