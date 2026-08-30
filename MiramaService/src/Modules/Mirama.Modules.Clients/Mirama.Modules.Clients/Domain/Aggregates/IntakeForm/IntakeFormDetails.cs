namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

public sealed record IntakeFormDetails(
    string Name,
    IReadOnlyList<IntakeFormField> Fields);
