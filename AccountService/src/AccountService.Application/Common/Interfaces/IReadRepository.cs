
using AccountService.Application.Domain.Abstractions.Core;

namespace AccountService.Application.Common.Interfaces;

public interface IReadRepository<T, TID> where T : AggregateRoot<TID>{
    IQueryable<T> Query();
}