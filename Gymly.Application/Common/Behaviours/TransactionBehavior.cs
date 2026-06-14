using Gymly.Application.Interfaces;
using MediatR;

namespace Gymly.Application.Common.Behaviours;

public class TransactionBehavior<TRequest, TResponse>(IApplicationDbContext context)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await context.ExecuteInTransactionAsync(
            async ct =>
            {
                var response = await next();
                return response;
            },
            cancellationToken);
    }
}