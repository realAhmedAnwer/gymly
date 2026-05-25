namespace Gymly.Domain.Entities.Users;

public class SystemRole : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<SystemUser> SystemUsers { get; set; } = new List<SystemUser>();
}
