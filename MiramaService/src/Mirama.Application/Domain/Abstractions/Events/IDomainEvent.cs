
using MediatR;

namespace Mirama.Application.Domain.Abstractions.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}