using Gymly.Web.Models.Bookings;

namespace Gymly.Web.Models.Attendance;

public class CheckInViewModel
{
    public string? QrToken { get; set; }
    public int? SelectedMemberId { get; set; }
    public List<MemberOption> AvailableMembers { get; set; } = [];
}