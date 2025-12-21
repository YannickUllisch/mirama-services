


using ErrorOr;

namespace AccountService.Application.Common.Interfaces;

public interface ICommandRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<ErrorOr<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}