using Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeFormSubmission;

public sealed record IntakeFormSubmissionDetails(
    IntakeFormId IntakeFormId,
    int IntakeFormVersion,
    IReadOnlyDictionary<string, string> Responses);
