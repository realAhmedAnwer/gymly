using System.ComponentModel.DataAnnotations;
using Gymly.Application.Features.Plans.Queries.GetPlanWithRules;
using Gymly.Domain.Enums;

namespace Gymly.Web.Models.Plans;

public class EditPlanViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Plan title is required.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99,999.99.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Duration is required.")]
    [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days.")]
    [Display(Name = "Duration (Days)")]
    public int DurationInDays { get; set; }

    public bool IsActive { get; set; }

    public List<PlanAccessRuleDto> AccessRules { get; set; } = [];

    [Display(Name = "New Rule Type")]
    public AccessType? NewRuleType { get; set; }

    [Display(Name = "New Rule Value")]
    public string? NewRuleValue { get; set; }
}