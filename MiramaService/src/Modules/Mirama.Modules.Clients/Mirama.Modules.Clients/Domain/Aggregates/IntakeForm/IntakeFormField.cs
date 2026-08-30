namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

// An owned value object, not an entity, it has no identity or lifecycle of its
// own independent of the form it belongs to. Equality is by value, which is
// exactly what a record gives here for free.
public sealed record IntakeFormField
{
    public string Label { get; }
    public IntakeFieldType Type { get; }
    public bool IsRequired { get; }
    public IReadOnlyList<string>? Options { get; }

    private IntakeFormField(string label, IntakeFieldType type, bool isRequired, IReadOnlyList<string>? options)
    {
        Label = label;
        Type = type;
        IsRequired = isRequired;
        Options = options;
    }

    public static IntakeFormField Create(string label, IntakeFieldType type, bool isRequired, IReadOnlyList<string>? options = null)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Field label cannot be empty.", nameof(label));

        if (type == IntakeFieldType.Select && (options is null || options.Count == 0))
            throw new ArgumentException("A Select field requires at least one option.", nameof(options));

        return new IntakeFormField(label.Trim(), type, isRequired, options);
    }
}
