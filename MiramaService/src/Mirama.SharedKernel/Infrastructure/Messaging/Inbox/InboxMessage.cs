
namespace Mirama.SharedKernel.Infrastructure.Messaging.Inbox;

public sealed class InboxMessage
{
    public Guid Id { get; init; }

    public DateTime? ProcessedAtUtc { get; set; }
}
