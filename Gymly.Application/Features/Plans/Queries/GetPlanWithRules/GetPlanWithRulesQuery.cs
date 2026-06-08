using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Queries.GetPlanWithRules;

public record GetPlanWithRulesQuery(int PlanId) : IRequest<PlanWithRulesDto?>;

public class GetPlanWithRulesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPlanWithRulesQuery, PlanWithRulesDto?>
{
    public async Task<PlanWithRulesDto?> Handle(GetPlanWithRulesQuery request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);

        if (plan == null) return null;

        var rules = await context.PlanAccessRules
            .AsNoTracking()
            .Where(r => r.PlanId == request.PlanId)
            .Select(r => new PlanAccessRuleDto(r.Id, r.PlanId, r.RuleType.ToString(), r.RuleValue))
            .ToListAsync(cancellationToken);

        return new PlanWithRulesDto(
            plan.Id,
            plan.Title,
            plan.Description,
            plan.Price,
            plan.DurationInDays,
            plan.IsActive,
            rules
        );
    }
}