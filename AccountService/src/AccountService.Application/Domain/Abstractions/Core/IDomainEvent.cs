
using MediatR;

namespace AccountService.Application.Domain.Abstractions.Core;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}