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

    private Policy(PolicyDetails details)
    {
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
        this.TenantId = details.TenantId;
        this.Scope = details.Scope;
        this.IsManaged = details.IsManaged;
    }

    private Policy() { }

    public static Policy Create(PolicyDetails details)
    {
        return new Policy(details) { Id = new PolicyId(Guid.NewGuid()) };
    }

    public void Update(string name, string? description)
    {
        this.Name = name.Trim();
        this.Description = description?.Trim();
    }

    public ErrorOr<PolicyStatement> AddStatement(string action, string resource = "*", Effect effect = Effect.Allow)
    {
        if (string.IsNullOrWhiteSpace(action))
            return Error.Validation("Policy.Statement.Action", "Action cannot be empty.");

        var statement = PolicyStatement.Create(Id, action.Trim(), resource.Trim(), effect);
        Statements.Add(statement);
        return statement;
    }

    public ErrorOr<Deleted> RemoveStatement(PolicyStatementId statementId)
    {
        var statement = Statements.Find(s => s.Id == statementId);
        if (statement is null)
            return Error.NotFound("Policy.Statement.NotFound", "Statement not found.");

        Statements.Remove(statement);
        return Result.Deleted;
    }
}
