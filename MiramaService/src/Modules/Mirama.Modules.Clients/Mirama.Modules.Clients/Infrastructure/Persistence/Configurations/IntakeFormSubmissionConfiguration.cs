using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;
using Mirama.Modules.Clients.Domain.Aggregates.IntakeFormSubmission;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class IntakeFormSubmissionConfiguration : IEntityTypeConfiguration<IntakeFormSubmission>
{
    public void Configure(EntityTypeBuilder<IntakeFormSubmission> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, v => new IntakeFormSubmissionId(v))
            .IsRequired();

        builder.Property(s => s.IntakeFormId)
            .HasConversion(id => id.Value, v => new IntakeFormId(v))
            .IsRequired();

        builder.Property(s => s.IntakeFormVersion).IsRequired();

        builder.Property(s => s.Responses)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null)!)
            .HasColumnType("jsonb")
            .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                (a, b) => a != null && b != null && a.SequenceEqual(b),
                v => v.Aggregate(0, (h, kv) => HashCode.Combine(h, kv.Key, kv.Value)),
                v => new Dictionary<string, string>(v)));

        builder.Property(s => s.Status).IsRequired();

        builder.Property(s => s.ConvertedToClientId)
            .HasConversion(
                id => id == null ? (Guid?)null : id.Value,
                v => v == null ? null : new ClientId(v.Value));

        builder.Property(s => s.SubmittedAt).IsRequired();
        builder.Property(s => s.OrganizationId).IsRequired();

        builder.HasIndex(s => s.OrganizationId);
        builder.HasIndex(s => s.IntakeFormId);
    }
}
