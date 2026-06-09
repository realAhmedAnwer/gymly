using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Members.Queries.GetMembers;

public record GetMembersQuery(
    bool? ShowInactive = null,
    string? SortBy = null,
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<MemberPagedResult>;

public record MemberPagedResult(
    List<MemberDto> Members,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public class GetMembersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMembersQuery, MemberPagedResult>
{
    public async Task<MemberPagedResult> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Members.AsNoTracking().AsQueryable();

        if (request.ShowInactive != true)
        {
            query = query.Where(m => m.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(m =>
                m.Name.ToLower().Contains(term) ||
                m.Email.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = request.SortBy?.ToLowerInvariant();
        query = sortBy switch
        {
            "email" => query.OrderBy(m => m.Email),
            "email_desc" => query.OrderByDescending(m => m.Email),
            "registrationdate" => query.OrderBy(m => m.RegistrationDate),
            "registrationdate_desc" => query.OrderByDescending(m => m.RegistrationDate),
            _ => query.OrderBy(m => m.Name)
        };

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = request.PageSize;

        var members = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MemberDto(
                m.Id,
                m.Name,
                m.Email,
                m.Phone,
                m.RegistrationDate,
                m.AttendanceCardToken,
                m.IsActive
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new MemberPagedResult(members, totalCount, pageNumber, pageSize, totalPages);
    }
}