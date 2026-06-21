using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using Gymly.Domain.Entities.Memberships;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Commands.CreatePlan;

public record CreatePlanCommand(
    string Title,
    string Description,
    decimal Price,
    int DurationInDays) : IRequest<int>;

public class CreatePlanCommandHandler(IApplicationDbContext context, ICacheService cacheService) : IRequestHandler<CreatePlanCommand, int>
{
    public async Task<int> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.Plans.AnyAsync(p => p.Title == request.Title, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("A plan with this title already exists.");
        }

        var plan = new Plan
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            DurationInDays = request.DurationInDays,
            IsActive = true
        };

        context.Plans.Add(plan);
        await context.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveByPrefixAsync(CacheKeys.AllPlans, cancellationToken);

        return plan.Id;
    }
}
