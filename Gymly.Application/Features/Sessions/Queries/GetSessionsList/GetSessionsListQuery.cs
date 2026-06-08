using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Sessions.Queries.GetSessionsList;

public record GetSessionsListQuery : IRequest<List<SessionDto>>;

public class GetSessionsListQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSessionsListQuery, List<SessionDto>>
{
    public async Task<List<SessionDto>> Handle(GetSessionsListQuery request, CancellationToken cancellationToken)
    {
        return await context.Sessions
            .AsNoTracking()
            .Select(s => new SessionDto(
                s.Id,
                s.Class!.Name,
                s.Trainer!.Name,
                s.StartTime,
                s.EndTime
            ))
            .ToListAsync(cancellationToken);
    }
}
