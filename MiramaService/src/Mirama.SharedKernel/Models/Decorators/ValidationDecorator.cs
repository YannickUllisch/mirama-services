
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Models.Decorators;

public class ValidationDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    ILogger<ValidationDecorator<TRequest, TResponse>> logger,
    IValidator<TRequest>? validator = null)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        if (request is IQuery)
        {
            return await inner.HandleAsync(request, ct);
        }

        if (validator is null)
        {
            logger.LogDebug("[Pipeline] No validator found for {RequestName}. Skipping.", requestName);
            return await inner.HandleAsync(request, ct);
        }

        // Execute Validation
        var validationResult = await validator.ValidateAsync(request, ct);

        // Handle Success
        if (validationResult.IsValid)
        {
            return await inner.HandleAsync(request, ct);
        }

        // Handle Failure & Map Errors
        var errors = validationResult.Errors
            .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
            .ToList();

        // Log the property names that failed
        var failedProperties = string.Join(", ", validationResult.Errors.Select(e => e.PropertyName));

        logger.LogWarning(
            "[Pipeline] Validation failed for {RequestName}. Errors in properties: {Properties}",
            requestName,
            failedProperties);

        return (dynamic)errors;
    }
}
