using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdAuditOperation).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TableName)
			.IsRequired()
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.PrimaryKey).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.OldValues).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.NewValues).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.AffectedColumns).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.AuditCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceFrame).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uniqueidentifier");

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
