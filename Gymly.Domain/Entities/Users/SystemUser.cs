namespace Gymly.Domain.Entities.Users;

public class SystemUser : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int SystemRoleId { get; set; }
    public SystemRole? SystemRole { get; set; }
}
