using Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

namespace Gymly.Web.Models.Trainers;

public class TrainersIndexViewModel
{
    public List<TrainerDto> Trainers { get; set; } = [];
    public int TotalTrainers => Trainers.Count;
}
