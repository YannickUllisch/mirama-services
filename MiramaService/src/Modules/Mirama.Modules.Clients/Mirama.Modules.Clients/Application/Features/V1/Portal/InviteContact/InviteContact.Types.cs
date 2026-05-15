using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Application.Features.V1.Portal.InviteContact;

public sealed record InviteContactCommand(
    Guid ClientId,
    Guid ContactId) : ICommand<ErrorOr<InvitationResponse>>;

public sealed record InvitationResponse(
    Guid InvitationId,
    Guid ContactId,
    DateTime ExpiresAt);
