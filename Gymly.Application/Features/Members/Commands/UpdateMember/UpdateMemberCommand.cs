using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Commands.UpdateMember;

public record UpdateMemberCommand(
    int MemberId,
    string Name,
    string Email,
    string Phone) : IRequest<bool>;

public class UpdateMemberCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateMemberCommand, bool>
{
    public async Task<bool> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var member = await context.Members.FirstOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);
        if (member == null) return false;

        var duplicate = await context.Members.AnyAsync(
            m => m.Email == normalizedEmail && m.Id != request.MemberId, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("Another member with this email address already exists.");
        }

        member.Name = request.Name;
        member.Email = normalizedEmail;
        member.Phone = request.Phone;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("Another member with this email address already exists.");
        }

        return true;
    }
}
