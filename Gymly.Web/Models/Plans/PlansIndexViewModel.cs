using Gymly.Application.Features.Plans.Queries.GetPlans;

namespace Gymly.Web.Models.Plans;

public class PlansIndexViewModel
{
    public List<PlanDto> Plans { get; set; } = [];
    public int TotalPlans => Plans.Count;
}