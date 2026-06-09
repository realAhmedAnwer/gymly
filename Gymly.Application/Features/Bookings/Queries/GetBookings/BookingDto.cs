namespace Gymly.Application.Features.Bookings.Queries.GetBookings;

public record BookingDto(
    int Id,
    int SessionId,
    string ClassName,
    int TrainerId,
    string TrainerName,
    int MemberId,
    string MemberName,
    DateTime StartTime,
    DateTime EndTime,
    DateTime BookedAt,
    bool IsCancelled
);