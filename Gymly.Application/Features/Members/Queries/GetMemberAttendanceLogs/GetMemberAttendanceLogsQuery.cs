using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Queries.GetMemberAttendanceLogs;

public record GetMemberAttendanceLogsQuery(int MemberId, int MaxCount = 20) : IRequest<List<AttendanceLogDto>>;

public class GetMemberAttendanceLogsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMemberAttendanceLogsQuery, List<AttendanceLogDto>>
{
    public async Task<List<AttendanceLogDto>> Handle(GetMemberAttendanceLogsQuery request, CancellationToken cancellationToken)
    {
        return await context.AttendanceLogs
            .AsNoTracking()
            .Where(a => a.MemberId == request.MemberId && !a.IsDeleted)
            .OrderByDescending(a => a.ScannedAt)
            .Take(request.MaxCount)
            .Select(a => new AttendanceLogDto(
                a.Id,
                a.ScannedAt,
                a.Method.ToString(),
                a.WasGranted,
                a.RejectionReason
            ))
            .ToListAsync(cancellationToken);
    }
}