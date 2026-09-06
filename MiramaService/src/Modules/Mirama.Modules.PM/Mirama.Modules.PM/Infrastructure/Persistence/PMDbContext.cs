using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Persistence;

namespace Mirama.Modules.PM.Infrastructure.Persistence;

public sealed class PMDbContext : AuditableUnitOfWorkDbContext
{
    protected override string SchemaName => "projects";

    public PMDbContext(
        DbContextOptions<PMDbContext> options,
        IDispatcher dispatcher,
        IRequestContextProvider requestContext) : base(options, dispatcher, requestContext)
    {
    }

    public PMDbContext() { }
}
