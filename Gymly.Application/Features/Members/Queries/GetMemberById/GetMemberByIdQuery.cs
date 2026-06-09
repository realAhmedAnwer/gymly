using Gymly.Application.Interfaces;
using Gymly.Application.Features.Members.Queries.GetMembers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Queries.GetMemberById;

public record GetMemberByIdQuery(int Id) : IRequest<MemberDto?>;

public class GetMemberByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMemberByIdQuery, MemberDto?>
{
    public async Task<MemberDto?> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Members
            .AsNoTracking()
            .Where(m => m.Id == request.Id)
            .Select(m => new MemberDto(
                m.Id,
                m.Name,
                m.Email,
                m.Phone,
                m.RegistrationDate,
                m.AttendanceCardToken,
                m.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}