using System.Globalization;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

public class IntakeForm : OrganizationAggregateRoot<IntakeFormId>
{
    public string Name { get; private set; } = string.Empty;
    public int Version { get; private set; }
    public bool IsActive { get; private set; }
    public List<IntakeFormField> Fields { get; private set; } = [];
    public List<IntakeFormSection> Sections { get; private set; } = [];

    private IntakeForm() { }

    private IntakeForm(IntakeFormDetails details)
    {
        this.Name = details.Name.Trim();
        this.Sections = [.. details.Sections ?? []];
        this.Fields = [.. details.Fields];
        this.Version = 1;
        this.IsActive = true;
    }

    public static IntakeForm Create(IntakeFormDetails details)
    {
        if (details.Fields.Count == 0)
            throw new ArgumentException("An intake form needs at least one field.", nameof(details));

        ValidateStructure(details.Fields, details.Sections ?? []);

        return new IntakeForm(details) { Id = new IntakeFormId(Guid.NewGuid()) };
    }

    // Revising fields bumps the version rather than mutating in place, so a
    // submission already on file stays interpretable against the field set it
    // was actually submitted against, rather than silently reinterpreted
    // against whatever the form looks like today.
    public void ReviseFields(IReadOnlyList<IntakeFormField> fields, IReadOnlyList<IntakeFormSection>? sections = null)
    {
        if (fields.Count == 0)
            throw new ArgumentException("An intake form needs at least one field.", nameof(fields));

        var resolvedSections = sections ?? this.Sections;
        ValidateStructure(fields, resolvedSections);

        this.Fields = [.. fields];
        this.Sections = [.. resolvedSections];
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

    // Referential integrity across the whole schema - a field can't point at
    // a section or a sibling field that doesn't exist, and conditions can't
    // form a cycle a UI would spin forever trying to resolve.
    private static void ValidateStructure(IReadOnlyList<IntakeFormField> fields, IReadOnlyList<IntakeFormSection> sections)
    {
        var fieldKeys = new HashSet<string>();
        foreach (var field in fields)
        {
            if (!fieldKeys.Add(field.Key))
                throw new ArgumentException($"Duplicate field key '{field.Key}'.", nameof(fields));
        }

        var sectionKeys = new HashSet<string>();
        foreach (var section in sections)
        {
            if (!sectionKeys.Add(section.Key))
                throw new ArgumentException($"Duplicate section key '{section.Key}'.", nameof(sections));
        }

        foreach (var field in fields)
        {
            if (field.SectionKey is not null && !sectionKeys.Contains(field.SectionKey))
                throw new ArgumentException($"Field '{field.Key}' references unknown section '{field.SectionKey}'.", nameof(fields));

            if (field.VisibleWhen is not null && !fieldKeys.Contains(field.VisibleWhen.DependsOnFieldKey))
                throw new ArgumentException($"Field '{field.Key}' depends on unknown field '{field.VisibleWhen.DependsOnFieldKey}'.", nameof(fields));
        }

        EnsureNoConditionCycles(fields);
    }

    private static void EnsureNoConditionCycles(IReadOnlyList<IntakeFormField> fields)
    {
        var dependsOn = fields
            .Where(f => f.VisibleWhen is not null)
            .ToDictionary(f => f.Key, f => f.VisibleWhen!.DependsOnFieldKey);

        foreach (var start in dependsOn.Keys)
        {
            var visited = new HashSet<string> { start };
            var current = start;

            while (dependsOn.TryGetValue(current, out var next))
            {
                if (!visited.Add(next))
                    throw new ArgumentException($"Field visibility conditions form a cycle involving '{next}'.");

                current = next;
            }
        }
    }

    // The compiler-enforced boundary of this whole scheme: the switch below
    // is exhaustive over IntakeFieldType, so a new case added to the enum
    // without a matching parse/validate branch here fails to build rather
    // than silently accepting unvalidated input at runtime. Everything above
    // Type (label, order, sections, presentation, options, thresholds) is
    // free for a tenant to reshape however they like; this is the one seam
    // where that freedom stops and a fixed, typed contract takes over.
    public IReadOnlyList<string> ValidateResponses(IReadOnlyDictionary<string, string> responses)
    {
        var errors = new List<string>();

        foreach (var field in this.Fields)
        {
            var isVisible = field.VisibleWhen is null || field.VisibleWhen.IsSatisfiedBy(responses);
            var hasValue = responses.TryGetValue(field.Key, out var raw) && !string.IsNullOrEmpty(raw);

            if (!isVisible)
                continue;

            if (!hasValue)
            {
                if (field.IsRequired)
                    errors.Add($"'{field.Label}' is required.");
                continue;
            }

            var error = ValidateValue(field, raw!);
            if (error is not null)
                errors.Add($"'{field.Label}': {error}");
        }

        return errors;
    }

    private static string? ValidateValue(IntakeFormField field, string raw) => field.Type switch
    {
        IntakeFieldType.Text or IntakeFieldType.LongText => ValidateText(field, raw),
        IntakeFieldType.Email => IsValidEmail(raw) ? ValidateText(field, raw) : "must be a valid email address.",
        IntakeFieldType.Phone => ValidateText(field, raw),
        IntakeFieldType.Url => Uri.TryCreate(raw, UriKind.Absolute, out _) ? ValidateText(field, raw) : "must be a valid URL.",
        IntakeFieldType.Number => ValidateNumber(field, raw),
        IntakeFieldType.Currency => ValidateNumber(field, raw),
        IntakeFieldType.Date => ValidateDate(field, raw),
        IntakeFieldType.Checkbox => bool.TryParse(raw, out _) ? null : "must be true or false.",
        IntakeFieldType.Select => ValidateSelect(field, raw),
        IntakeFieldType.MultiSelect => ValidateMultiSelect(field, raw),
        _ => throw new ArgumentOutOfRangeException(nameof(field), field.Type, "Unhandled field type.")
    };

    private static string? ValidateText(IntakeFormField field, string raw)
    {
        var rule = field.Validation;
        if (rule?.MinLength is { } min && raw.Length < min)
            return $"must be at least {min} characters.";
        if (rule?.MaxLength is { } max && raw.Length > max)
            return $"must be at most {max} characters.";
        if (rule?.Pattern is { } pattern && !System.Text.RegularExpressions.Regex.IsMatch(raw, pattern))
            return "is not in the expected format.";
        return null;
    }

    private static string? ValidateNumber(IntakeFormField field, string raw)
    {
        if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
            return "must be a number.";

        var rule = field.Validation;
        if (rule?.MinValue is { } min && value < min)
            return $"must be at least {min}.";
        if (rule?.MaxValue is { } max && value > max)
            return $"must be at most {max}.";
        return null;
    }

    private static string? ValidateDate(IntakeFormField field, string raw)
    {
        if (!DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var value))
            return "must be a valid date.";

        var rule = field.Validation;
        if (rule?.MinDate is { } min && value < min)
            return $"must be on or after {min:yyyy-MM-dd}.";
        if (rule?.MaxDate is { } max && value > max)
            return $"must be on or before {max:yyyy-MM-dd}.";
        return null;
    }

    private static string? ValidateSelect(IntakeFormField field, string raw)
    {
        return field.Options is not null && field.Options.Contains(raw)
            ? null
            : "is not one of the allowed options.";
    }

    private static string? ValidateMultiSelect(IntakeFormField field, string raw)
    {
        List<string>? selections;
        try
        {
            selections = System.Text.Json.JsonSerializer.Deserialize<List<string>>(raw);
        }
        catch (System.Text.Json.JsonException)
        {
            return "must be a JSON array of selected options.";
        }

        if (selections is null or { Count: 0 })
            return "must include at least one selected option.";

        if (field.Options is not null && selections.Any(s => !field.Options.Contains(s)))
            return "contains an option that is not allowed.";

        var rule = field.Validation;
        if (rule?.MinSelections is { } min && selections.Count < min)
            return $"requires at least {min} selections.";
        if (rule?.MaxSelections is { } max && selections.Count > max)
            return $"allows at most {max} selections.";

        return null;
    }

    private static bool IsValidEmail(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.Contains('@') && !value.StartsWith('@') && !value.EndsWith('@');
}
