using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Sessions;

public class CreateSessionViewModel
{
    [Required(ErrorMessage = "Please select a class.")]
    [Display(Name = "Class")]
    public int ClassId { get; set; }

    [Required(ErrorMessage = "Please assign a trainer.")]
    [Display(Name = "Trainer")]
    public int TrainerId { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    [Display(Name = "Start Date & Time")]
    [DataType(DataType.DateTime)]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "End time is required.")]
    [Display(Name = "End Date & Time")]
    [DataType(DataType.DateTime)]
    public DateTime EndTime { get; set; } = DateTime.UtcNow.AddHours(1);
    public List<SelectListItem> AvailableClasses { get; set; } = [];
    public List<SelectListItem> AvailableTrainers { get; set; } = [];
}