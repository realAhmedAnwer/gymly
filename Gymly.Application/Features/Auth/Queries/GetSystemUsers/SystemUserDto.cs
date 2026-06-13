namespace Gymly.Application.Features.Auth.Queries.GetSystemUsers;

public record SystemUserDto(int Id, string Username, string Email, string FullName, bool IsActive, string Role);