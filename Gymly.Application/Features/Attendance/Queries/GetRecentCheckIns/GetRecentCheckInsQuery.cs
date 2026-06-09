using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Attendance.Queries.GetRecentCheckIns;

public record GetRecentCheckInsQuery(int Count = 50) : IRequest<List<RecentCheckInDto>>;

public class GetRecentCheckInsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetRecentCheckInsQuery, List<RecentCheckInDto>>
{
    public async Task<List<RecentCheckInDto>> Handle(GetRecentCheckInsQuery request, CancellationToken cancellationToken)
    {
        return await context.AttendanceLogs
            .AsNoTracking()
            .OrderByDescending(a => a.ScannedAt)
            .Take(request.Count)
            .Select(a => new RecentCheckInDto
            {
                Id = a.Id,
                MemberId = a.MemberId,
                MemberName = a.Member!.Name,
                ScannedAt = a.ScannedAt,
                Method = a.Method.ToString(),
                WasGranted = a.WasGranted,
                RejectionReason = a.RejectionReason
            })
            .ToListAsync(cancellationToken);
    }
}