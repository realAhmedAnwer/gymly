using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Queries.GetPlans;

public record GetPlansQuery(bool? ShowInactive = null, string? SortBy = null) : IRequest<List<PlanDto>>;

public class GetPlansQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPlansQuery, List<PlanDto>>
{
    public async Task<List<PlanDto>> Handle(GetPlansQuery request, CancellationToken cancellationToken)
    {
        var plansQuery = context.Plans.AsNoTracking().AsQueryable();

        if (request.ShowInactive != true)
        {
            plansQuery = plansQuery.Where(p => p.IsActive);
        }

        plansQuery = request.SortBy?.ToLowerInvariant() switch
        {
            "price" => plansQuery.OrderBy(p => p.Price),
            "price_desc" => plansQuery.OrderByDescending(p => p.Price),
            "duration" => plansQuery.OrderBy(p => p.DurationInDays),
            "duration_desc" => plansQuery.OrderByDescending(p => p.DurationInDays),
            _ => plansQuery.OrderBy(p => p.Title)
        };

        var plans = await plansQuery.ToListAsync(cancellationToken);

        var planIds = plans.Select(p => p.Id).ToList();
        var rules = await context.PlanAccessRules
            .AsNoTracking()
            .Where(r => planIds.Contains(r.PlanId))
            .Select(r => new { r.PlanId, r.Id, r.RuleType, r.RuleValue })
            .ToListAsync(cancellationToken);

        var rulesByPlan = rules
            .GroupBy(r => r.PlanId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => new PlanAccessRuleSummaryDto(r.Id, r.RuleType.ToString(), r.RuleValue)).ToList()
            );

        return plans.Select(p => new PlanDto(
            p.Id,
            p.Title,
            p.Description,
            p.Price,
            p.DurationInDays,
            p.IsActive,
            rulesByPlan.TryGetValue(p.Id, out var r) ? r : []
        )).ToList();
    }
}