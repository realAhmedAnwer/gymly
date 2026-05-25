namespace Gymly.Domain.Entities.Users;

public class Trainer : User
{
    public string Specialization { get; set; } = string.Empty;
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
}
