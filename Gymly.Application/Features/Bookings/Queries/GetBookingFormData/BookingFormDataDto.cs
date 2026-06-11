namespace Gymly.Application.Features.Bookings.Queries.GetBookingFormData;

public class BookingFormDataDto
{
    public List<SessionOptionDto> AvailableSessions { get; set; } = [];
    public List<MemberOptionDto> AvailableMembers { get; set; } = [];
}

public class SessionOptionDto
{
    public int Id { get; set; }
    public string Display { get; set; } = string.Empty;
    public int BookedCount { get; set; }
    public int MaxCapacity { get; set; }
    public bool IsFull => BookedCount >= MaxCapacity && MaxCapacity > 0;
}

public class MemberOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}