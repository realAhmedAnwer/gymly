using Gymly.Domain.Enums;

namespace Gymly.Domain.Entities.Schedules;

public class AttendanceLog : BaseEntity
{
    public int MemberId { get; set; }
    public DateTime ScannedAt { get; set; } = DateTime.UtcNow;
    public AccessMethod Method { get; set; }
    public bool WasGranted { get; set; }
    public string RejectionReason { get; set; } = string.Empty;
}