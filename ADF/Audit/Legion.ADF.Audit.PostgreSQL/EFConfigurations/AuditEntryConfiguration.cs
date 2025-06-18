using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.PostgreSQL;

public class AuditEntryConfiguration : IEntityTypeConfiguration<Audit.Model.AuditEntry>
{
	public const string PrimaryKeyFormatter = "{{\"IdAuditEntry\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Audit.Model.AuditEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.AuditEntry> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAuditEntry);

		entityBuilder.ToTable("AuditEntry", "aud");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_AuditEntry_CorrelationId");

		entityBuilder.HasIndex(e => e.IdAuditOperation, "IXFK_AuditEntry_AuditOperation");

		entityBuilder.Property(e => e.IdAuditEntry)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditOperation).HasColumnType("uuid");

		entityBuilder.Property(e => e.TableName)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.PrimaryKey).HasColumnType("jsonb");

		entityBuilder.Property(e => e.OldValues).HasColumnType("jsonb");

		entityBuilder.Property(e => e.NewValues).HasColumnType("jsonb");

		entityBuilder.Property(e => e.AffectedColumns).HasColumnType("jsonb");

		entityBuilder.Property(e => e.AuditCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.AuditOperation)
			.WithMany(p => p.AuditEntries)
			.HasForeignKey(d => d.IdAuditOperation)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AuditEntry_IdAuditOperation");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Audit.Model.AuditEntry>(ConfigureEntity);

		return modelBuilder;
	}
}
