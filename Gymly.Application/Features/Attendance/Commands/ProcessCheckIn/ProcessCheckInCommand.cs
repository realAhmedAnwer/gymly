using Gymly.Application.Interfaces;
using Gymly.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Attendance.Commands.ProcessCheckIn;

public record ProcessCheckInCommand(Guid? QrToken, int? MemberId, AccessMethod Method) : IRequest<CheckInResult>;

public class ProcessCheckInCommandHandler(IApplicationDbContext context)
    : IRequestHandler<ProcessCheckInCommand, CheckInResult>
{
    public async Task<CheckInResult> Handle(ProcessCheckInCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Users.Member? member;

        if (request.QrToken.HasValue)
        {
            member = await context.Members
                .FirstOrDefaultAsync(m => m.AttendanceCardToken == request.QrToken.Value, cancellationToken);
        }
        else if (request.MemberId.HasValue)
        {
            member = await context.Members
                .FirstOrDefaultAsync(m => m.Id == request.MemberId.Value, cancellationToken);
        }
        else
        {
            return new CheckInResult
            {
                WasGranted = false,
                RejectionReason = "No member identifier provided."
            };
        }

        if (member == null)
        {
            return new CheckInResult
            {
                WasGranted = false,
                RejectionReason = "Member not found."
            };
        }

        if (!member.IsActive)
        {
            return await LogAndReturn(member, request.Method, false, "Member account is deactivated.", cancellationToken);
        }

        var activeMembership = await context.Memberships
            .Include(m => m.Plan)
            .Where(m => m.MemberId == member.Id
                && m.Status == Domain.Enums.MembershipStatus.Active
                && m.EndDate >= DateTime.UtcNow)
            .OrderByDescending(m => m.StartDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (activeMembership == null)
        {
            return await LogAndReturn(member, request.Method, false, "No active membership found.", cancellationToken);
        }

        if (activeMembership.Plan != null && !activeMembership.Plan.IsActive)
        {
            return await LogAndReturn(member, request.Method, false, "Associated membership plan is not active.", cancellationToken);
        }

        return await LogAndReturn(member, request.Method, true, string.Empty, cancellationToken);
    }

    private async Task<CheckInResult> LogAndReturn(Domain.Entities.Users.Member member, AccessMethod method, bool granted, string reason, CancellationToken cancellationToken)
    {
        var log = new Domain.Entities.Schedules.AttendanceLog
        {
            MemberId = member.Id,
            ScannedAt = DateTime.UtcNow,
            Method = method,
            WasGranted = granted,
            RejectionReason = reason
        };

        context.AttendanceLogs.Add(log);
        await context.SaveChangesAsync(cancellationToken);

        return new CheckInResult
        {
            WasGranted = granted,
            RejectionReason = reason,
            MemberId = member.Id,
            MemberName = member.Name
        };
    }
}