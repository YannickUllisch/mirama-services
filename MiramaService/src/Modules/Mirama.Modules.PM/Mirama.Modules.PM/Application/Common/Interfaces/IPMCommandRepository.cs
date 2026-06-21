

using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Application.Common.Interfaces;

public interface IPMCommandRepository<T, TID> where T : AggregateRoot<TID>
{
    IQueryable<T> Query();
}