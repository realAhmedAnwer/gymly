namespace Gymly.Web.Models.Attendance;

public class CheckInViewModel
{
    public string? QrToken { get; set; }
    public string? SearchTerm { get; set; }
    public int? SelectedMemberId { get; set; }
}
