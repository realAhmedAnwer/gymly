using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gymly.Web.Models.Sessions;

public class CreateSessionViewModel
{
    public int ClassId { get; set; }
    public int TrainerId { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);
    public List<SelectListItem> AvailableClasses { get; set; } = [];
    public List<SelectListItem> AvailableTrainers { get; set; } = [];
}