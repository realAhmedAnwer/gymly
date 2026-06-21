using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Queries.GetTrainersLookup;

public record GetTrainersQuery : IRequest<List<TrainerLookupDto>>;

public class GetTrainersQueryHandler(IApplicationDbContext context, ICacheService cacheService)
    : IRequestHandler<GetTrainersQuery, List<TrainerLookupDto>>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public async Task<List<TrainerLookupDto>> Handle(GetTrainersQuery request, CancellationToken cancellationToken)
    {
        var cachedResult = await cacheService.GetAsync<List<TrainerLookupDto>>(CacheKeys.AllTrainers, cancellationToken);
        if (cachedResult is not null)
            return cachedResult;

        var result = await context.Trainers
            .AsNoTracking()
            .Select(t => new TrainerLookupDto(
                t.Id,
                t.Name,
                t.Specialization
            ))
            .ToListAsync(cancellationToken);

        await cacheService.SetAsync(CacheKeys.AllTrainers, result, CacheDuration, cancellationToken);
        return result;
    }
}

public record TrainerLookupDto(int Id, string Name, string Specialization);