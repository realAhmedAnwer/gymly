using Gymly.Domain.Enums;

namespace Gymly.Domain.Entities.Memberships;

public class PlanAccessRule : BaseEntity
{
    public int PlanId { get; set; }
    public AccessType RuleType { get; set; }
    public string RuleValue { get; set; } = string.Empty;
}