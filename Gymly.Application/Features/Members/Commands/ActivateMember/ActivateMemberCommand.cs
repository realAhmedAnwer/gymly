using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Commands.ActivateMember;

public record ActivateMemberCommand(int MemberId) : IRequest<bool>;

public class ActivateMemberCommandHandler(IApplicationDbContext context) : IRequestHandler<ActivateMemberCommand, bool>
{
    public async Task<bool> Handle(ActivateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await context.Members.FirstOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);
        if (member == null) return false;

        member.IsActive = true;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}