using Gymly.Application.Interfaces;
using Gymly.Domain.Entities.Memberships;
using Gymly.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Commands.AddPlanAccessRule;

public record AddPlanAccessRuleCommand(int PlanId, AccessType RuleType, string RuleValue) : IRequest<int>;

public class AddPlanAccessRuleCommandHandler(IApplicationDbContext context) : IRequestHandler<AddPlanAccessRuleCommand, int>
{
    public async Task<int> Handle(AddPlanAccessRuleCommand request, CancellationToken cancellationToken)
    {
        var planExists = await context.Plans.AnyAsync(p => p.Id == request.PlanId, cancellationToken);
        if (!planExists)
        {
            throw new ArgumentException("The specified plan does not exist.");
        }

        var rule = new PlanAccessRule
        {
            PlanId = request.PlanId,
            RuleType = request.RuleType,
            RuleValue = request.RuleValue
        };

        context.PlanAccessRules.Add(rule);
        await context.SaveChangesAsync(cancellationToken);

        return rule.Id;
    }
}