using System.Diagnostics;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.SharedKernel.Models.Decorators;

public sealed class AuditDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    IAuditLogger auditLogger,
    IRequestContextProvider context)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        if (request is not IQuery)
            return await inner.HandleAsync(request, ct);

        var operationName = typeof(TRequest).Name;
        var traceId = Activity.Current?.TraceId.ToString() ?? string.Empty;

        string userId;
        try { userId = context.UserId.ToString(); }
        catch (UnauthorizedAccessException) { userId = "anonymous"; }

        var result = await inner.HandleAsync(request, ct);

        auditLogger.LogRead(
            operationName,
            userId,
            context.TenantId?.ToString(),
            context.OrganizationId?.ToString(),
            context.ProjectId?.ToString(),
            result.IsError ? "Failed" : "Succeeded",
            traceId);

        return result;
    }
}
