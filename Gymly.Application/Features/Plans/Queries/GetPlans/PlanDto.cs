namespace Gymly.Application.Features.Plans.Queries.GetPlans;

public record PlanAccessRuleSummaryDto(int Id, string RuleType, string RuleValue);

public record PlanDto(
    int Id,
    string Title,
    string Description,
    decimal Price,
    int DurationInDays,
    bool IsActive,
    List<PlanAccessRuleSummaryDto> AccessRules
);