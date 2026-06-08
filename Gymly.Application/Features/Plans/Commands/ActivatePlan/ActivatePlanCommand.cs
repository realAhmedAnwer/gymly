using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Commands.ActivatePlan;

public record ActivatePlanCommand(int PlanId) : IRequest<bool>;

public class ActivatePlanCommandHandler(IApplicationDbContext context) : IRequestHandler<ActivatePlanCommand, bool>
{
    public async Task<bool> Handle(ActivatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans.FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);
        if (plan == null) return false;

        plan.IsActive = true;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}