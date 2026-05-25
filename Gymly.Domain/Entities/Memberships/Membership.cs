using Gymly.Domain.Entities.Users;
using Gymly.Domain.Enums;

namespace Gymly.Domain.Entities.Memberships;

public class Membership : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
    public int PlanId { get; set; }
    public Plan? Plan { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MembershipStatus Status { get; set; } = MembershipStatus.Active;
    public bool IsCurrentlyValid()
    {
        return Status == MembershipStatus.Active && DateTime.UtcNow <= EndDate;
    }
}
