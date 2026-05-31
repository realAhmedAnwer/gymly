using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Classes;

public class CreateClassViewModel
{
    [Required(ErrorMessage = "Class name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Maximum room capacity is required.")]
    [Range(1, 200, ErrorMessage = "Capacity must be between 1 and 200 attendees.")]
    [Display(Name = "Max Capacity")]
    public int MaxCapacity { get; set; } = 20;
}