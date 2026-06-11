using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Queries.GetTrainersLookup;

public record GetTrainersQuery : IRequest<List<TrainerLookupDto>>;

public class GetTrainersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTrainersQuery, List<TrainerLookupDto>>
{
    public async Task<List<TrainerLookupDto>> Handle(GetTrainersQuery request, CancellationToken cancellationToken)
    {
        return await context.Trainers
            .AsNoTracking()
            .Select(t => new TrainerLookupDto(
                t.Id,
                t.Name,
                t.Specialization
            ))
            .ToListAsync(cancellationToken);
    }
}

public record TrainerLookupDto(int Id, string Name, string Specialization);