using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;

namespace Gymly.Application.Features.Classes.Commands.DeleteClass;

public record DeleteClassCommand(int ClassId) : IRequest<bool>;

public class DeleteClassCommandHandler(IApplicationDbContext context, ICacheService cacheService) : IRequestHandler<DeleteClassCommand, bool>
{
    public async Task<bool> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        var fitnessClass = await context.Classes.FindAsync([request.ClassId], cancellationToken);
        if (fitnessClass == null) return false;

        context.Classes.Remove(fitnessClass);
        await context.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveByPrefixAsync(CacheKeys.AllClasses, cancellationToken);

        return true;
    }
}
