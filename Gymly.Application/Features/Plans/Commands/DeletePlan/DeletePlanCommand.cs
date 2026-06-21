using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Commands.DeletePlan;

public record DeletePlanCommand(int PlanId) : IRequest<bool>;

public class DeletePlanCommandHandler(IApplicationDbContext context, ICacheService cacheService) : IRequestHandler<DeletePlanCommand, bool>
{
    public async Task<bool> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans.FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);
        if (plan == null) return false;

        plan.IsActive = false;
        await context.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveByPrefixAsync(CacheKeys.AllPlans, cancellationToken);

        return true;
    }
}
