namespace Gymly.Application.Features.Sessions.Queries.GetSessionsList;

public record SessionDto(
    int Id,
    string ClassName,
    string TrainerName,
    DateTime StartTime,
    DateTime EndTime,
    int BookedCount = 0,
    int MaxCapacity = 0
);
