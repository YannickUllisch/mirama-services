
using Mirama.Application.Domain.Abstractions.Core;

namespace Mirama.Application.Common.Interfaces;

public interface IReadRepository<T, TID> where T : AggregateRoot<TID>{
    IQueryable<T> Query();
}