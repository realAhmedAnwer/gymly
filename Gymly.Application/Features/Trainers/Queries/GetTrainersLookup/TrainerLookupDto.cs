namespace Gymly.Application.Features.Trainers.Queries.GetTrainersLookup;

public class TrainerLookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
}