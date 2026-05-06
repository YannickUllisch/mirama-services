using ErrorOr;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Policy;

public sealed class Policy : AggregateRoot<PolicyId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsManaged { get; private set; }
    public Guid? TenantId { get; private set; }
    public AccessScope Scope { get; private set; }

    public List<PolicyStatement> Statements { get; private set; } = [];

    private Policy() { }

    public static Policy Create(string name, string? description, Guid? tenantId, AccessScope scope, bool isManaged = false)
    {
        return new Policy
        {
            Id = new PolicyId(Guid.NewGuid()),
            Name = name.Trim(),
            Description = description?.Trim(),
            TenantId = tenantId,
            Scope = scope,
            IsManaged = isManaged
        };
    }

    public ErrorOr<PolicyStatement> AddStatement(string action, string resource = "*", Effect effect = Effect.Allow)
    {
        if (string.IsNullOrWhiteSpace(action))
            return Error.Validation("Policy.Statement.Action", "Action cannot be empty.");

        var statement = PolicyStatement.Create(Id, action.Trim(), resource.Trim(), effect);
        this.Statements.Add(statement);
        return statement;
    }

    public ErrorOr<Deleted> RemoveStatement(PolicyStatementId statementId)
    {
        var statement = this.Statements.Find(s => s.Id == statementId);
        if (statement is null)
        {
            return Error.NotFound("Policy.Statement.NotFound", "Statement not found.");
        }

        this.Statements.Remove(statement);
        return Result.Deleted;
    }

    public void Update(string name, string? description)
    {
        this.Name = name.Trim();
        this.Description = description?.Trim();
    }
}
