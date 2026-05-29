
using System.Diagnostics;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Models.Decorators;

public class PerformanceDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    ILogger<PerformanceDecorator<TRequest, TResponse>> logger)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private const int ThresholdMs = 300;

    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await inner.HandleAsync(request, ct);

        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        if (elapsedMilliseconds > ThresholdMs)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogWarning(
                "[Performance] Long Running Request: {Name} ({ElapsedMilliseconds} ms)",
                requestName,
                elapsedMilliseconds);
        }

        return response;
    }
}
