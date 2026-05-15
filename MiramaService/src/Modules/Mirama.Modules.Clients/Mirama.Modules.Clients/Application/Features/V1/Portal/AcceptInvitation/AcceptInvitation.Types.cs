using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Application.Features.V1.Portal.AcceptInvitation;

public sealed record AcceptInvitationCommand(Guid Token) : ICommand<ErrorOr<PortalSessionResponse>>;

public sealed record PortalSessionResponse(
    Guid ClientPortalUserId,
    Guid ClientId,
    string AccessToken);
