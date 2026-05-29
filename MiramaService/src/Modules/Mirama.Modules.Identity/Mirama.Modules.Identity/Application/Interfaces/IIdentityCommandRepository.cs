

using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Application.Common.Interfaces;

public interface IIdentityCommandRepository<T, TID> where T : AggregateRoot<TID>
{
    IQueryable<T> Query();
}