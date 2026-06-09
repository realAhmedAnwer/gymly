using Gymly.Application.Features.Bookings.Queries.GetBookings;

namespace Gymly.Web.Models.Bookings;

public class BookingsIndexViewModel
{
    public List<BookingDto> Bookings { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool? ShowCancelled { get; set; }
    public int? FilterSessionId { get; set; }
    public int? FilterMemberId { get; set; }
}