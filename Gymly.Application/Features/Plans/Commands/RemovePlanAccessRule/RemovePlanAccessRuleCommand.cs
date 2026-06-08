using Gymly.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Plans.Commands.RemovePlanAccessRule;

public record RemovePlanAccessRuleCommand(int RuleId) : IRequest<bool>;

public class RemovePlanAccessRuleCommandHandler(IApplicationDbContext context) : IRequestHandler<RemovePlanAccessRuleCommand, bool>
{
    public async Task<bool> Handle(RemovePlanAccessRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = await context.PlanAccessRules.FirstOrDefaultAsync(r => r.Id == request.RuleId, cancellationToken);
        if (rule == null) return false;

        context.PlanAccessRules.Remove(rule);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}