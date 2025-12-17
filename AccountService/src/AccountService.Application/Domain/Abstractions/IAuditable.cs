
namespace AccountService.Application.Domain.Abstractions;

public interface IAuditable
{
    public DateTime Created { get; }
    public string? CreatedBy { get; }
    public DateTime? LastModified { get; }
    public string? LastModifiedBy { get; }

    void SetCreated(DateTime created, string? createdBy);
    void SetModified(DateTime lastModified, string? lastModifiedBy);
}
