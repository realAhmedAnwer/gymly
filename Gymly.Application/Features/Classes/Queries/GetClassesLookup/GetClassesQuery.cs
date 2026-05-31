using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Classes.Queries.GetClassesLookup;

public record GetClassesQuery : IRequest<List<ClassLookupDto>>;

public class GetClassesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetClassesQuery, List<ClassLookupDto>>
{
    public async Task<List<ClassLookupDto>> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
        return await context.Classes
            .AsNoTracking()
            .Select(c => new ClassLookupDto(c.Id, c.Name))
            .ToListAsync(cancellationToken);
    }
}