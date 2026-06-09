namespace Gymly.Application.Features.Members.Queries.GetMemberAttendanceLogs;

public record AttendanceLogDto(
    int Id,
    DateTime ScannedAt,
    string Method,
    bool WasGranted,
    string RejectionReason
);