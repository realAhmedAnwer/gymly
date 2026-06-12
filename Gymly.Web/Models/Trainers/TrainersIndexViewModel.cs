using Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

namespace Gymly.Web.Models.Trainers;

public class TrainersIndexViewModel
{
    public List<TrainerDto> Trainers { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalCount { get; set; }
    public int PageSize { get; set; } = 10;
}