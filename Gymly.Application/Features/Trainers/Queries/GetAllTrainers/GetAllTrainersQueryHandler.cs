using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

public class GetAllTrainersQueryHandler : IRequestHandler<GetAllTrainersQuery, List<TrainerDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTrainersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TrainerDto>> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Trainers
            .AsNoTracking()
            .Select(t => new TrainerDto(
                t.Id,
                t.Name,
                t.Email,
                t.Phone,
                t.Specialization,
                t.HireDate
            ))
            .ToListAsync(cancellationToken);
    }
}
