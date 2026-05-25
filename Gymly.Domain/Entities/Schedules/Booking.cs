using Gymly.Domain.Entities.Users;

namespace Gymly.Domain.Entities.Schedules;

public class Booking : BaseEntity
{
    public int SessionId { get; set; }
    public Session Session { get; set; } = default!;
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public bool IsCancelled { get; set; } = false;
}
