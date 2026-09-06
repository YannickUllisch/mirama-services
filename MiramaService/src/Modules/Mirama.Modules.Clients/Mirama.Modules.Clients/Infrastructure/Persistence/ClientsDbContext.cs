using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalInvitation;
using Mirama.Modules.Clients.Domain.Aggregates.Client.ClientPortalUser;
using Mirama.Modules.Clients.Domain.Aggregates.Client.Contact;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Persistence;

namespace Mirama.Modules.Clients.Infrastructure.Persistence;

public sealed class ClientsDbContext : AuditableUnitOfWorkDbContext
{
    protected override string SchemaName => "clients";

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<ClientPortalUser> PortalUsers => Set<ClientPortalUser>();
    public DbSet<ClientPortalInvitation> PortalInvitations => Set<ClientPortalInvitation>();

    public ClientsDbContext(
        DbContextOptions<ClientsDbContext> options,
        IDispatcher dispatcher,
        IRequestContextProvider requestContext) : base(options, dispatcher, requestContext)
    {
    }

    public ClientsDbContext() { }
}
