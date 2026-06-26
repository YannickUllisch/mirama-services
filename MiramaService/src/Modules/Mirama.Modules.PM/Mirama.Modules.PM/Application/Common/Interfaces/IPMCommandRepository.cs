using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Application.Common.Interfaces;

public interface IPMCommandRepository<T, TID> where T : Entity<TID>
{
    IQueryable<T> Query();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}