using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

public class IntakeForm : OrganizationAggregateRoot<IntakeFormId>
{
    public string Name { get; private set; } = string.Empty;
    public int Version { get; private set; }
    public bool IsActive { get; private set; }
    public List<IntakeFormField> Fields { get; private set; } = [];

    private IntakeForm() { }

    private IntakeForm(IntakeFormDetails details)
    {
        this.Name = details.Name.Trim();
        this.Fields = [.. details.Fields];
        this.Version = 1;
        this.IsActive = true;
    }

    public static IntakeForm Create(IntakeFormDetails details)
    {
        if (details.Fields.Count == 0)
            throw new ArgumentException("An intake form needs at least one field.", nameof(details));

        return new IntakeForm(details) { Id = new IntakeFormId(Guid.NewGuid()) };
    }

    // Revising fields bumps the version rather than mutating in place, so a
    // submission already on file stays interpretable against the field set it
    // was actually submitted against, rather than silently reinterpreted
    // against whatever the form looks like today.
    public void ReviseFields(IReadOnlyList<IntakeFormField> fields)
    {
        if (fields.Count == 0)
            throw new ArgumentException("An intake form needs at least one field.", nameof(fields));

        this.Fields = [.. fields];
        this.Version++;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        this.Name = name.Trim();
    }

    public void Deactivate() => this.IsActive = false;

    public void Reactivate() => this.IsActive = true;
}
