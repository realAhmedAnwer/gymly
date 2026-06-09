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
        var member = await context.Members.FirstOrDefaultAsync(m => m.Id == request.MemberId, cancellationToken);
        if (member == null) return false;

        var duplicate = await context.Members.AnyAsync(
            m => m.Email == request.Email && m.Id != request.MemberId, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("Another member with this email address already exists.");
        }

        member.Name = request.Name;
        member.Email = request.Email;
        member.Phone = request.Phone;

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}