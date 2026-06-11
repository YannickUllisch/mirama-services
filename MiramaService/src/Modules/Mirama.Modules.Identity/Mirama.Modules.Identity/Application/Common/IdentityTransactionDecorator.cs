using ErrorOr;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Common;

internal class IdentityTransactionDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    IdentityDbContext dbContext)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        if (request is not ICommand)
            return await inner.HandleAsync(request, ct);

        await dbContext.BeginTransactionAsync(ct);
        var response = await inner.HandleAsync(request, ct);

        if (response.IsError)
        {
            await dbContext.RollbackTransactionAsync(ct);
            return response;
        }

        await dbContext.SaveChangesAsync(ct);
        await dbContext.CommitTransactionAsync(ct);
        return response;
    }
}
