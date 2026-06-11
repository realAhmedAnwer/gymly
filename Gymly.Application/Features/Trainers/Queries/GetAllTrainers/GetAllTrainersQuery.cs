using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

public record GetAllTrainersQuery : IRequest<List<TrainerDto>>;

public class GetAllTrainersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetAllTrainersQuery, List<TrainerDto>>
{
    public async Task<List<TrainerDto>> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
    {
        return await context.Trainers
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