
using MediatR;

namespace AccountService.Application.Domain.Abstractions.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}