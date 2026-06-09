namespace Gymly.Application.Features.Members.Queries.GetMemberMembership;

public record MemberMembershipDto(
    int Id,
    int MemberId,
    int PlanId,
    string PlanTitle,
    decimal PlanPrice,
    int PlanDurationDays,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    bool IsCurrentlyValid
);