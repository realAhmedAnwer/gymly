using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
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
            .Include(s => s.Class)
            .Include(s => s.Trainer)
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