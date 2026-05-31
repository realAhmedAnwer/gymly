using Gymly.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Gymly.Application.Features.Trainers.Queries.GetTrainersLookup;

public class GetTrainersQueryHandler : IRequestHandler<GetTrainersQuery, IEnumerable<TrainerLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTrainersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrainerLookupDto>> Handle(GetTrainersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Trainers
            .Select(t => new TrainerLookupDto
            {
                Id = t.Id,
                Name = t.Name,
                Specialization = t.Specialization
            })
            .ToListAsync(cancellationToken);
    }
}