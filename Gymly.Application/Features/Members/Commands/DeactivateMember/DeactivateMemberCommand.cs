using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Commands.DeactivateMember;

public record DeactivateMemberCommand(int MemberId) : IRequest<bool>;

public class DeactivateMemberCommandHandler(IApplicationDbContext context) : IRequestHandler<DeactivateMemberCommand, bool>
{
    public async Task<bool> Handle(DeactivateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await context.Members.FirstOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);
        if (member == null) return false;

        member.IsActive = false;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}