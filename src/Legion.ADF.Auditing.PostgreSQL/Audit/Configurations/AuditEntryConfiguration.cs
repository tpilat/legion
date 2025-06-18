using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auditing.PostgreSQL.Audit;

public class AuditEntryConfiguration : IEntityTypeConfiguration<Auditing.Audit.AuditEntry>
{
	public const string PrimaryKeyFormatter = "{{\"IdAuditEntry\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auditing.Audit.AuditEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auditing.Audit.AuditEntry> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAuditEntry);

		entityBuilder.ToTable("AuditEntry", "aud");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_AuditEntry_CorrelationId");

		entityBuilder.HasIndex(e => e.CreatedUtc, "IX_AuditEntry_CreatedUtc");

		entityBuilder.HasIndex(e => e.IdAuditType, "IX_AuditEntry_IdAuditType");

		entityBuilder.Property(e => e.IdAuditEntry).HasDefaultValueSql("uuid_generate_v4()");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.TableName)
			.IsRequired()
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CommandQueryName).HasMaxLength(1023);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auditing.Audit.AuditEntry>(ConfigureEntity);

		return modelBuilder;
	}
}
