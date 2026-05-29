
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Abstractions.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}