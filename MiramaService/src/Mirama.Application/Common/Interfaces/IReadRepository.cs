
using Mirama.Domain.Abstractions.Core;

namespace Mirama.Application.Common.Interfaces;

public interface IReadRepository<T, TID> where T : Entity<TID>{
    IQueryable<T> Query();
}