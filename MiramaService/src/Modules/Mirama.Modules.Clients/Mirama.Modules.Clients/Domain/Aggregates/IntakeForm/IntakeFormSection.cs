namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

// Ordering is positional within IntakeForm.Sections - no separate Order
// field to drift out of sync with the list it's supposed to describe.
public sealed record IntakeFormSection
{
    public string Key { get; }
    public string Title { get; }
    public string? Description { get; }

    private IntakeFormSection(string key, string title, string? description)
    {
        Key = key;
        Title = title;
        Description = description;
    }

    public static IntakeFormSection Create(string key, string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Section key cannot be empty.", nameof(key));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Section title cannot be empty.", nameof(title));

        return new IntakeFormSection(key.Trim(), title.Trim(), string.IsNullOrWhiteSpace(description) ? null : description.Trim());
    }
}
