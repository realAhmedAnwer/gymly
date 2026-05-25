namespace Gymly.Domain.Entities.Users;

public class Member : User
{
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public Guid AttendanceCardToken { get; set; } = Guid.NewGuid();
    public bool IsActive { get; set; } = true;
    public bool IsEligibleForCheckIn()
    {
        return IsActive;
    }
}
