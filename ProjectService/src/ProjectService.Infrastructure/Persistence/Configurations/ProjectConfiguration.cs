
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Entities;
using ProjectService.Domain.ValueObjects;

namespace ProjectService.Infrastructure.Persistence.Configurations;

internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        // ID as Primary Key
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(
            projectId => projectId.Value,
            val => new ProjectId(val));
    }
}