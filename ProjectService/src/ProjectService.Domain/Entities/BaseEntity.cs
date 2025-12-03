


namespace ProjectService.Domain.Entities;

public class BaseEntity<TID>
{
    public TID Id { get; protected set; } = default!;

    public DateTime CreatedAt { get; private set; }

    public DateTime ModifiedAt { get; private set; }
}