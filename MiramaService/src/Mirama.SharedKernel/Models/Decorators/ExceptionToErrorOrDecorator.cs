
using ErrorOr;
using Microsoft.Extensions.Logging;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Models.Decorators;

public class ExceptionToErrorOrDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    ILogger<ExceptionToErrorOrDecorator<TRequest, TResponse>> logger)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        try
        {
            return await inner.HandleAsync(request, ct);
        }
        // catch (DomainException ex)
        // {
        //     // Map our custom Domain exceptions to ErrorOr types based on your mapping table
        //     Error error = ex switch
        //     {
        //         DomainNotFoundException => Error.NotFound(ex.Code, ex.Message),
        //         DomainValidationException => Error.Validation(ex.Code, ex.Message),
        //         BusinessRuleException => Error.Validation(ex.Code, ex.Message),
        //         DomainConflictException => Error.Conflict(ex.Code, ex.Message),
        //         DomainForbiddenException => Error.Forbidden(ex.Code, ex.Message),
        //         _ => Error.Unexpected(ex.Code, ex.Message)
        //     };

        //     logger.LogWarning(ex, "Domain error handled: {Code} ({ExceptionType})", ex.Code, ex.GetType().Name);

        //     // Convert to TResponse (ErrorOr) via dynamic dispatch
        //     return (dynamic)error;
        // }
        catch (Exception ex)
        {
            // This catches technical failures (DB connection, NullReference, etc.)
            logger.LogError(ex, "Unhandled system exception during {RequestName}", typeof(TRequest).Name);

            return (dynamic)Error.Unexpected(
                code: "General.UnhandledException",
                description: "An internal server error occurred.");
        }
    }
}
