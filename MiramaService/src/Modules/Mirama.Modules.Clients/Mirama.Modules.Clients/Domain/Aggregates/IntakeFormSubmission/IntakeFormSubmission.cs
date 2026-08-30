using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;
using Mirama.Modules.Clients.Domain.Events;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeFormSubmission;

// Its own aggregate root rather than a child of IntakeForm or Client, a
// submission is created independently of either, and needs a lifecycle
// (new, converted, discarded) that outlives any single client relationship,
// including submissions that never become a client at all.
public class IntakeFormSubmission : OrganizationAggregateRoot<IntakeFormSubmissionId>
{
    public IntakeFormId IntakeFormId { get; init; } = null!;
    public int IntakeFormVersion { get; init; }
    public Dictionary<string, string> Responses { get; private set; } = [];
    public IntakeFormSubmissionStatus Status { get; private set; }
    public ClientId? ConvertedToClientId { get; private set; }
    public DateTime SubmittedAt { get; private set; }

    private IntakeFormSubmission() { }

    private IntakeFormSubmission(IntakeFormSubmissionDetails details)
    {
        this.IntakeFormId = details.IntakeFormId;
        this.IntakeFormVersion = details.IntakeFormVersion;
        this.Responses = new Dictionary<string, string>(details.Responses);
        this.Status = IntakeFormSubmissionStatus.New;
        this.SubmittedAt = DateTime.UtcNow;
    }

    public static IntakeFormSubmission Create(IntakeFormSubmissionDetails details)
    {
        var submission = new IntakeFormSubmission(details) { Id = new IntakeFormSubmissionId(Guid.NewGuid()) };
        submission.AddDomainEvent(new IntakeFormSubmitted(submission.Id.Value, details.IntakeFormId.Value));
        return submission;
    }

    // The submitted payload itself is never edited, what came in is what came
    // in. Only processing status moves, and only forward, once.
    public void MarkConverted(ClientId clientId)
    {
        if (this.Status != IntakeFormSubmissionStatus.New)
            throw new InvalidOperationException("Only a new submission can be converted.");

        this.Status = IntakeFormSubmissionStatus.Converted;
        this.ConvertedToClientId = clientId;
        AddDomainEvent(new IntakeFormSubmissionConverted(Id.Value, clientId.Value));
    }

    public void Discard()
    {
        if (this.Status != IntakeFormSubmissionStatus.New)
            throw new InvalidOperationException("Only a new submission can be discarded.");

        this.Status = IntakeFormSubmissionStatus.Discarded;
    }
}
