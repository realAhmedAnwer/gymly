namespace Gymly.Application.Features.Members.Queries.GetMembers;

public record MemberDto(
    int Id,
    string Name,
    string Email,
    string Phone,
    DateTime RegistrationDate,
    Guid AttendanceCardToken,
    bool IsActive
);