namespace Gymly.Domain.Entities.Schedules;

public class Class : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
}