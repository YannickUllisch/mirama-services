

using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.SharedKernel.Abstractions.Persistence;

public interface ICommandRepository<T, TID> where T : AggregateRoot<TID>
{
    IQueryable<T> Query();
}