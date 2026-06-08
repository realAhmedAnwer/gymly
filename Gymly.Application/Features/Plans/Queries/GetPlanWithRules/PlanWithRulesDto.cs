using Gymly.Application.Features.Plans.Queries.GetPlans;

namespace Gymly.Application.Features.Plans.Queries.GetPlanWithRules;

public record PlanAccessRuleDto(int Id, int PlanId, string RuleType, string RuleValue);

public record PlanWithRulesDto(
    int Id,
    string Title,
    string Description,
    decimal Price,
    int DurationInDays,
    bool IsActive,
    List<PlanAccessRuleDto> AccessRules
)
{
    public static PlanWithRulesDto From(PlanDto plan, List<PlanAccessRuleDto> rules) =>
        new(plan.Id, plan.Title, plan.Description, plan.Price, plan.DurationInDays, plan.IsActive, rules);
}