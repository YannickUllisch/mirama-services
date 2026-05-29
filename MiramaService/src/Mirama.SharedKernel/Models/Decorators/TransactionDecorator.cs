using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.SharedKernel.Models.Decorators;

public class TransactionDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> inner,
    IUnitOfWork unitOfWork)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct)
    {
        // Queries never require an explicit transaction.
        if (request is not ICommand)
        {
            return await inner.HandleAsync(request, ct);
        }

        // Single Unit of Work: wraps handler execution + SaveChanges + domain event extraction
        // in one atomic transaction. Prevents partial writes if SaveChanges or Outbox fails.
        await unitOfWork.BeginTransactionAsync(ct);

        var response = await inner.HandleAsync(request, ct);

        if (response.IsError)
        {
            await unitOfWork.RollbackTransactionAsync(ct);
            return response;
        }

        await unitOfWork.SaveChangesAsync(ct);
        await unitOfWork.CommitTransactionAsync(ct);

        return response;
    }
}
