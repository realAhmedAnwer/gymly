using Gymly.Application.Interfaces;
using Gymly.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Memberships.Commands.AssignMemberToPlan;

public record AssignMemberToPlanCommand(int MemberId, int PlanId, DateTime StartDate, int? DurationInDays) : IRequest<int>;

public class AssignMemberToPlanCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AssignMemberToPlanCommand, int>
{
    public async Task<int> Handle(AssignMemberToPlanCommand request, CancellationToken cancellationToken)
    {
        var member = await context.Members
            .FirstOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);

        if (member == null)
        {
            throw new InvalidOperationException("Member not found.");
        }

        if (!member.IsActive)
        {
            throw new InvalidOperationException("Cannot assign a plan to an inactive member.");
        }

        var plan = await context.Plans
            .FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);

        if (plan == null)
        {
            throw new InvalidOperationException("Plan not found.");
        }

        if (!plan.IsActive)
        {
            throw new InvalidOperationException("Cannot assign an inactive plan.");
        }

        var startDate = request.StartDate.Kind == DateTimeKind.Utc
            ? request.StartDate
            : request.StartDate.ToUniversalTime();

        var duration = request.DurationInDays ?? plan.DurationInDays;
        var endDate = startDate.AddDays(duration);

        var membership = new Domain.Entities.Memberships.Membership
        {
            MemberId = request.MemberId,
            PlanId = request.PlanId,
            StartDate = startDate,
            EndDate = endDate,
            Status = MembershipStatus.Active
        };

        context.Memberships.Add(membership);
        await context.SaveChangesAsync(cancellationToken);

        return membership.Id;
    }
}