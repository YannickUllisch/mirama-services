
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Application.Common.Interfaces;

public interface IPMQueryRepository<T, TID> where T : Entity<TID>
{
    IQueryable<T> Query();
}