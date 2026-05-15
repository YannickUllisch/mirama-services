using ErrorOr;
using Mirama.Modules.Clients.Application.Features.V1.Clients.CreateClient;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Application.Features.V1.Clients.GetClients;

public sealed record GetClientsQuery : IQuery<ErrorOr<List<ClientResponse>>>;
