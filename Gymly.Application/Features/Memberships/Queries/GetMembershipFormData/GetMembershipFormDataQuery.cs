using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Memberships.Queries.GetMembershipFormData;

public record GetMembershipFormDataQuery(int MemberId) : IRequest<MembershipFormDataDto?>;

public class GetMembershipFormDataQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMembershipFormDataQuery, MembershipFormDataDto?>
{
    public async Task<MembershipFormDataDto?> Handle(GetMembershipFormDataQuery request, CancellationToken cancellationToken)
    {
        var member = await context.Members
            .AsNoTracking()
            .Where(m => m.Id == request.MemberId && m.IsActive)
            .Select(m => new { m.Id, m.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (member == null) return null;

        var plans = await context.Plans
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Title)
            .Select(p => new PlanOptionDto
            {
                Id = p.Id,
                Title = p.Title,
                DurationInDays = p.DurationInDays
            })
            .ToListAsync(cancellationToken);

        return new MembershipFormDataDto
        {
            MemberId = member.Id,
            MemberName = member.Name,
            AvailablePlans = plans
        };
    }
}