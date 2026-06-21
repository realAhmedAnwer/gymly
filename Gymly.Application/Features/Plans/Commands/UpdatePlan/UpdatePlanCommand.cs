using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Commands.UpdatePlan;

public record UpdatePlanCommand(
    int PlanId,
    string Title,
    string Description,
    decimal Price,
    int DurationInDays) : IRequest<bool>;

public class UpdatePlanCommandHandler(IApplicationDbContext context, ICacheService cacheService) : IRequestHandler<UpdatePlanCommand, bool>
{
    public async Task<bool> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans.FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);
        if (plan == null) return false;

        var duplicate = await context.Plans.AnyAsync(
            p => p.Title == request.Title && p.Id != request.PlanId, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("Another plan with this title already exists.");
        }

        plan.Title = request.Title;
        plan.Description = request.Description;
        plan.Price = request.Price;
        plan.DurationInDays = request.DurationInDays;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("Another plan with this title already exists.");
        }

        await cacheService.RemoveByPrefixAsync(CacheKeys.AllPlans, cancellationToken);

        return true;
    }
}
