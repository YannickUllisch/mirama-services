namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

// A single flat slot set rather than a per-type subclass hierarchy - which
// members apply is decided by IntakeFormField.Create against the field's
// Type, so a Number field can never carry a Pattern and a Text field can
// never carry a MinValue. This keeps storage/serialization simple (one
// shape, straight through EF's owned-JSON mapping) while Create is what
// actually enforces the type-to-constraint contract.
public sealed record IntakeFieldValidationRule
{
    public int? MinLength { get; }
    public int? MaxLength { get; }
    public string? Pattern { get; }
    public decimal? MinValue { get; }
    public decimal? MaxValue { get; }
    public DateTime? MinDate { get; }
    public DateTime? MaxDate { get; }
    public int? MinSelections { get; }
    public int? MaxSelections { get; }

    private IntakeFieldValidationRule(
        int? minLength, int? maxLength, string? pattern,
        decimal? minValue, decimal? maxValue,
        DateTime? minDate, DateTime? maxDate,
        int? minSelections, int? maxSelections)
    {
        MinLength = minLength;
        MaxLength = maxLength;
        Pattern = pattern;
        MinValue = minValue;
        MaxValue = maxValue;
        MinDate = minDate;
        MaxDate = maxDate;
        MinSelections = minSelections;
        MaxSelections = maxSelections;
    }

    public static IntakeFieldValidationRule Create(
        int? minLength = null, int? maxLength = null, string? pattern = null,
        decimal? minValue = null, decimal? maxValue = null,
        DateTime? minDate = null, DateTime? maxDate = null,
        int? minSelections = null, int? maxSelections = null)
    {
        if (minLength.HasValue && maxLength.HasValue && minLength > maxLength)
            throw new ArgumentException("MinLength cannot exceed MaxLength.", nameof(minLength));

        if (minValue.HasValue && maxValue.HasValue && minValue > maxValue)
            throw new ArgumentException("MinValue cannot exceed MaxValue.", nameof(minValue));

        if (minDate.HasValue && maxDate.HasValue && minDate > maxDate)
            throw new ArgumentException("MinDate cannot exceed MaxDate.", nameof(minDate));

        if (minSelections.HasValue && maxSelections.HasValue && minSelections > maxSelections)
            throw new ArgumentException("MinSelections cannot exceed MaxSelections.", nameof(minSelections));

        return new IntakeFieldValidationRule(
            minLength, maxLength, pattern, minValue, maxValue, minDate, maxDate, minSelections, maxSelections);
    }
}
