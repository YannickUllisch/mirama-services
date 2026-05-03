
using Mirama.Domain.Abstractions.Core;

namespace Mirama.Application.Common.Interfaces;

public interface ICommandRepository<T, TID> where T : AggregateRoot<TID>{
    IQueryable<T> Query();
}