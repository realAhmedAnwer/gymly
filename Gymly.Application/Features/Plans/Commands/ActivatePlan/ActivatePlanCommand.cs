using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Commands.ActivatePlan;

public record ActivatePlanCommand(int PlanId) : IRequest<bool>;

public class ActivatePlanCommandHandler(IApplicationDbContext context, ICacheService cacheService) : IRequestHandler<ActivatePlanCommand, bool>
{
    public async Task<bool> Handle(ActivatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans.FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);
        if (plan == null) return false;

        plan.IsActive = true;
        await context.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveByPrefixAsync(CacheKeys.AllPlans, cancellationToken);

        return true;
    }
}
