namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

// An owned value object, not an entity - it has no identity or lifecycle of
// its own independent of the form it belongs to. Equality is by value, which
// is exactly what a record gives here for free.
//
// Key is the stable machine name a Response is keyed by (IntakeFormSubmission
// stores Dictionary<Key, string>). Label is free text and can be reworded on
// every revision without invalidating submissions already on file; Key
// cannot, which is why it's validated as a slug rather than accepted as-is.
public sealed record IntakeFormField
{
    public string Key { get; private set; } = string.Empty;
    public string Label { get; private set; } = string.Empty;
    public IntakeFieldType Type { get; private set; }
    public bool IsRequired { get; private set; }
    public IReadOnlyList<string>? Options { get; private set; }
    public IntakeFieldValidationRule? Validation { get; private set; }
    public IntakeFormFieldPresentation Presentation { get; private set; } = IntakeFormFieldPresentation.Default;
    public string? SectionKey { get; private set; }
    public IntakeFieldCondition? VisibleWhen { get; private set; }

    // EF materializes owned navigations (Validation/Presentation/VisibleWhen)
    // by constructing via this parameterless constructor and setting
    // properties by reflection - constructor binding doesn't support
    // navigation properties, only scalars.
    private IntakeFormField() { }

    private IntakeFormField(
        string key, string label, IntakeFieldType type, bool isRequired,
        IReadOnlyList<string>? options, IntakeFieldValidationRule? validation,
        IntakeFormFieldPresentation presentation, string? sectionKey, IntakeFieldCondition? visibleWhen)
    {
        Key = key;
        Label = label;
        Type = type;
        IsRequired = isRequired;
        Options = options;
        Validation = validation;
        Presentation = presentation;
        SectionKey = sectionKey;
        VisibleWhen = visibleWhen;
    }

    public static IntakeFormField Create(
        string key,
        string label,
        IntakeFieldType type,
        bool isRequired,
        IReadOnlyList<string>? options = null,
        IntakeFieldValidationRule? validation = null,
        IntakeFormFieldPresentation? presentation = null,
        string? sectionKey = null,
        IntakeFieldCondition? visibleWhen = null)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Field label cannot be empty.", nameof(label));

        if (!IsValidKey(key))
            throw new ArgumentException("Field key must be lowercase letters, digits, and hyphens, e.g. 'company-name'.", nameof(key));

        var isChoiceType = type is IntakeFieldType.Select or IntakeFieldType.MultiSelect;
        if (isChoiceType && (options is null || options.Count == 0))
            throw new ArgumentException($"A {type} field requires at least one option.", nameof(options));

        if (!isChoiceType && options is { Count: > 0 })
            throw new ArgumentException($"Options are not applicable to a {type} field.", nameof(options));

        if (visibleWhen?.DependsOnFieldKey == key)
            throw new ArgumentException("A field cannot depend on itself.", nameof(visibleWhen));

        if (validation is not null)
            EnsureValidationApplies(type, validation);

        return new IntakeFormField(
            key.Trim(), label.Trim(), type, isRequired, options, validation,
            presentation ?? IntakeFormFieldPresentation.Default, sectionKey, visibleWhen);
    }

    private static bool IsValidKey(string key) =>
        !string.IsNullOrWhiteSpace(key)
        && key.All(c => char.IsAsciiLetterLower(c) || char.IsAsciiDigit(c) || c == '-')
        && char.IsAsciiLetterLower(key[0]);

    // The exhaustive switch is the point: adding a new IntakeFieldType without
    // updating this method is a compile error, not a silently-accepted
    // constraint nobody enforces.
    private static void EnsureValidationApplies(IntakeFieldType type, IntakeFieldValidationRule validation)
    {
        var allowed = type switch
        {
            IntakeFieldType.Text or IntakeFieldType.LongText or IntakeFieldType.Email
                or IntakeFieldType.Phone or IntakeFieldType.Url =>
                validation is { MinValue: null, MaxValue: null, MinDate: null, MaxDate: null, MinSelections: null, MaxSelections: null },

            IntakeFieldType.Number or IntakeFieldType.Currency =>
                validation is { MinLength: null, MaxLength: null, Pattern: null, MinDate: null, MaxDate: null, MinSelections: null, MaxSelections: null },

            IntakeFieldType.Date =>
                validation is { MinLength: null, MaxLength: null, Pattern: null, MinValue: null, MaxValue: null, MinSelections: null, MaxSelections: null },

            IntakeFieldType.Checkbox =>
                validation is
                {
                    MinLength: null, MaxLength: null, Pattern: null, MinValue: null, MaxValue: null,
                    MinDate: null, MaxDate: null, MinSelections: null, MaxSelections: null
                },

            IntakeFieldType.Select =>
                validation is
                {
                    MinLength: null, MaxLength: null, Pattern: null, MinValue: null, MaxValue: null,
                    MinDate: null, MaxDate: null, MinSelections: null, MaxSelections: null
                },

            IntakeFieldType.MultiSelect =>
                validation is { MinLength: null, MaxLength: null, Pattern: null, MinValue: null, MaxValue: null, MinDate: null, MaxDate: null },

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unhandled field type.")
        };

        if (!allowed)
            throw new ArgumentException($"One or more validation constraints do not apply to a {type} field.", nameof(validation));
    }
}
