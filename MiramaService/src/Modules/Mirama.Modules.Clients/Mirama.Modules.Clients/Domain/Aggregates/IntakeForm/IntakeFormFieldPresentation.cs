namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

// Pure rendering hints - never consulted by validation. Free to change on
// every revision without touching field identity, type, or constraints.
public sealed record IntakeFormFieldPresentation
{
    public string? Placeholder { get; }
    public string? HelpText { get; }
    public IntakeFieldWidth Width { get; }

    private IntakeFormFieldPresentation(string? placeholder, string? helpText, IntakeFieldWidth width)
    {
        Placeholder = placeholder;
        HelpText = helpText;
        Width = width;
    }

    public static IntakeFormFieldPresentation Create(
        string? placeholder = null,
        string? helpText = null,
        IntakeFieldWidth width = IntakeFieldWidth.Full)
    {
        return new IntakeFormFieldPresentation(
            string.IsNullOrWhiteSpace(placeholder) ? null : placeholder.Trim(),
            string.IsNullOrWhiteSpace(helpText) ? null : helpText.Trim(),
            width);
    }

    public static readonly IntakeFormFieldPresentation Default = Create();
}
