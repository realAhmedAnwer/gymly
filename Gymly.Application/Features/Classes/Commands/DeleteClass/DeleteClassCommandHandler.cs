using Gymly.Application.Interfaces;
using MediatR;

namespace Gymly.Application.Features.Classes.Commands.DeleteClass;

public class DeleteClassCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteClassCommand, bool>
{
    public async Task<bool> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        var fitnessClass = await context.Classes.FindAsync([request.ClassId], cancellationToken);
        if (fitnessClass == null) return false;

        context.Classes.Remove(fitnessClass);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}