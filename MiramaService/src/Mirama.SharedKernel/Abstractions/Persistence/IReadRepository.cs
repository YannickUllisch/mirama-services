
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.SharedKernel.Abstractions.Persistence;

public interface IReadRepository<T, TID> where T : Entity<TID>
{
    IQueryable<T> Query();
}