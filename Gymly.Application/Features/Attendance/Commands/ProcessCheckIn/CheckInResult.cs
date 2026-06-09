namespace Gymly.Application.Features.Attendance.Commands.ProcessCheckIn;

public record CheckInResult
{
    public bool WasGranted { get; init; }
    public string RejectionReason { get; init; } = string.Empty;
    public int MemberId { get; init; }
    public string MemberName { get; init; } = string.Empty;
}