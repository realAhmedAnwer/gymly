namespace Gymly.Domain.Entities.Memberships;

public class Plan : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<PlanAccessRule> AccessRules { get; set; } = [];
}
