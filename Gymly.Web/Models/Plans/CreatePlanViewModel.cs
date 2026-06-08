using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Plans;

public class CreatePlanViewModel
{
    [Required(ErrorMessage = "Plan title is required.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99,999.99.")]
    public decimal Price { get; set; } = 29.99m;

    [Required(ErrorMessage = "Duration is required.")]
    [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days.")]
    [Display(Name = "Duration (Days)")]
    public int DurationInDays { get; set; } = 30;
}