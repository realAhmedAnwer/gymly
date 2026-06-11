using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Queries.SearchMembers;

public record SearchMembersQuery(string Term, int Limit = 8) : IRequest<List<MemberLookupDto>>;

public record MemberLookupDto(int Id, string Name, string Email, string Phone, bool IsActive);

public class SearchMembersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<SearchMembersQuery, List<MemberLookupDto>>
{
    public async Task<List<MemberLookupDto>> Handle(SearchMembersQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Term))
        {
            return [];
        }

        var term = request.Term.Trim().ToLower();

        return await context.Members
            .AsNoTracking()
            .Where(m =>
                m.Name.ToLower().Contains(term) ||
                m.Email.ToLower().Contains(term) ||
                (m.Phone != null && m.Phone.ToLower().Contains(term)))
            .OrderBy(m => m.Name)
            .Take(Math.Clamp(request.Limit, 1, 25))
            .Select(m => new MemberLookupDto(
                m.Id,
                m.Name,
                m.Email,
                m.Phone ?? string.Empty,
                m.IsActive))
            .ToListAsync(cancellationToken);
    }
}
