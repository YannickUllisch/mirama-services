namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

// A closed vocabulary of layout hints rather than a raw CSS/grid value, so
// the UI stays free to render however it likes (grid, flex, stacked cards)
// while the backend never stores presentation logic it can't reason about.
public enum IntakeFieldWidth
{
    Full,
    Half,
    Third,
    Quarter
}
