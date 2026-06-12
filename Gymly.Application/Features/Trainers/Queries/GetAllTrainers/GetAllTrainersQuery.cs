using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Queries.GetAllTrainers;

public record GetAllTrainersQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<TrainerPagedResult>;

public record TrainerPagedResult(
    List<TrainerDto> Trainers,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public class GetAllTrainersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetAllTrainersQuery, TrainerPagedResult>
{
    public async Task<TrainerPagedResult> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Trainers.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = request.PageSize;

        var trainers = await query
            .OrderBy(t => t.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TrainerDto(
                t.Id,
                t.Name,
                t.Email,
                t.Phone,
                t.Specialization,
                t.HireDate
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new TrainerPagedResult(trainers, totalCount, pageNumber, pageSize, totalPages);
    }
}
