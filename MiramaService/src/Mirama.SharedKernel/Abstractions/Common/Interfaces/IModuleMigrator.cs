namespace Mirama.SharedKernel.Abstractions.Common.Interfaces;

public interface IModuleMigrator
{
    string ModuleName { get; }
    Task MigrateAsync(CancellationToken cancellationToken = default);
}
