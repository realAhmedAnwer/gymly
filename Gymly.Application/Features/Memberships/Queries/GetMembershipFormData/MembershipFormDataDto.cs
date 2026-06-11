namespace Gymly.Application.Features.Memberships.Queries.GetMembershipFormData;

public class MembershipFormDataDto
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public List<PlanOptionDto> AvailablePlans { get; set; } = [];
}

public class PlanOptionDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationInDays { get; set; }
}