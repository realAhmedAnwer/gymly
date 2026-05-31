using Gymly.Domain.Entities.Users;

namespace Gymly.Web.Models.Trainers;

public class TrainersIndexViewModel
{
    public IEnumerable<Trainer> Trainers { get; set; } = [];
    public int TotalTrainers => Trainers.Count();
}