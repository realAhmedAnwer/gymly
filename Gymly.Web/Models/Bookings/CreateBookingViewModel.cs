using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Bookings;

public class CreateBookingViewModel
{
    [Required(ErrorMessage = "Please select a session.")]
    public int SessionId { get; set; }

    [Required(ErrorMessage = "Please select a member.")]
    public int MemberId { get; set; }

    public List<SessionOption> AvailableSessions { get; set; } = [];
    public List<MemberOption> AvailableMembers { get; set; } = [];
}

public class SessionOption
{
    public int Id { get; set; }
    public string Display { get; set; } = string.Empty;
    public int BookedCount { get; set; }
    public int MaxCapacity { get; set; }
    public bool IsFull => BookedCount >= MaxCapacity;
}

public class MemberOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}