
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Application.Common.Interfaces;

public interface IIdentityQueryRepository<T, TID> where T : Entity<TID>
{
    IQueryable<T> Query();
}