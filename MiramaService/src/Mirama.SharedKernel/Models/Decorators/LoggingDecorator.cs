
using ErrorOr;
using Microsoft.Extensions.Logging;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Models.Decorators;

public class LoggingDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    ILogger<LoggingDecorator<TRequest, TResponse>> logger)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        // Use a Log Scope to attach RequestName to every log entry inside this execution
        using (logger.BeginScope(new Dictionary<string, object> { ["RequestName"] = requestName }))
        {
            var result = await inner.HandleAsync(request, ct);

            if (result.IsError)
            {
                logger.LogWarning(
                    "Request {RequestName} failed with {ErrorCount} errors",
                    requestName,
                    result.Errors!.Count);
            }

            return result;
        }
    }
}
