using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gymly.Application.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handling {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;

        if (elapsedMs > 500)
        {
            logger.LogWarning(
                "Long running request: {RequestName} ({ElapsedMilliseconds} ms) {@Request}",
                requestName,
                elapsedMs,
                request);
        }
        else
        {
            logger.LogInformation(
                "Handled {RequestName} in {ElapsedMilliseconds} ms",
                requestName,
                elapsedMs);
        }

        return response;
    }
}