using Gymly.Application.Features.Attendance.Queries.GetRecentCheckIns;

namespace Gymly.Web.Models.Attendance;

public class AttendanceIndexViewModel
{
    public CheckInViewModel CheckIn { get; set; } = new();
    public List<RecentCheckInDto> RecentCheckIns { get; set; } = [];
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
}
