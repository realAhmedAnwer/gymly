namespace Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

public record TrainerDto(
    int Id,
    string Name,
    string Email,
    string Phone,
    string Specialization,
    DateTime HireDate
);