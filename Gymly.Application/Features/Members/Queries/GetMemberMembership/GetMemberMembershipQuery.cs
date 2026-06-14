using Gymly.Application.Interfaces;
using Gymly.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Queries.GetMemberMembership;

public record GetMemberMembershipQuery(int MemberId) : IRequest<MemberMembershipDto?>;

public class GetMemberMembershipQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMemberMembershipQuery, MemberMembershipDto?>
{
    public async Task<MemberMembershipDto?> Handle(GetMemberMembershipQuery request, CancellationToken cancellationToken)
    {
        var membership = await context.Memberships
            .AsNoTracking()
            .Where(m => m.MemberId == request.MemberId && !m.IsDeleted)
            .OrderByDescending(m => m.StartDate)
            .Select(m => new MemberMembershipDto(
                m.Id,
                m.MemberId,
                m.PlanId,
                m.Plan!.Title,
                m.Plan.Price,
                m.Plan.DurationInDays,
                m.StartDate,
                m.EndDate,
                m.Status.ToString(),
                m.Status == MembershipStatus.Active && DateTime.UtcNow >= m.StartDate && DateTime.UtcNow <= m.EndDate
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return membership;
    }
}