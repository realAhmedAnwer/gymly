using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Memberships;

public class AssignMembershipViewModel
{
    public int MemberId { get; set; }

    [Display(Name = "Member")]
    public string? MemberName { get; set; }

    [Required(ErrorMessage = "Please select a plan.")]
    [Display(Name = "Plan")]
    public int PlanId { get; set; }

    [Required(ErrorMessage = "Please enter a start date.")]
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    [Range(1, 3650, ErrorMessage = "Duration must be between 1 and 3650 days.")]
    [Display(Name = "Duration (days)")]
    public int? DurationInDays { get; set; }

    public List<PlanOption> AvailablePlans { get; set; } = [];
}

public class PlanOption
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationInDays { get; set; }
}