namespace Gymly.Application.Features.Attendance.Queries.GetRecentCheckIns;

public record RecentCheckInDto
{
    public int Id { get; init; }
    public int MemberId { get; init; }
    public string MemberName { get; init; } = string.Empty;
    public DateTime ScannedAt { get; init; }
    public string Method { get; init; } = string.Empty;
    public bool WasGranted { get; init; }
    public string RejectionReason { get; init; } = string.Empty;
}