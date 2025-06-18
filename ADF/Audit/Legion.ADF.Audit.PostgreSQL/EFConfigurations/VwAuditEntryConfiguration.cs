using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.PostgreSQL;

public class VwAuditEntryConfiguration : IEntityTypeConfiguration<Audit.Model.VwAuditEntry>
{
	public void Configure(EntityTypeBuilder<Audit.Model.VwAuditEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.VwAuditEntry> entityBuilder)
	{
		entityBuilder.ToView("VwAuditEntry", "aud")
			.HasNoKey();

		entityBuilder.Property(e => e.IdAuditEntry).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditOperation).HasColumnType("uuid");

		entityBuilder.Property(e => e.TableName)
			.IsRequired()
			.HasColumnType("varchar(255)");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.PrimaryKey).HasColumnType("jsonb");

		entityBuilder.Property(e => e.OldValues).HasColumnType("jsonb");

		entityBuilder.Property(e => e.NewValues).HasColumnType("jsonb");

		entityBuilder.Property(e => e.AffectedColumns).HasColumnType("jsonb");

		entityBuilder.Property(e => e.AuditCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Audit.Model.VwAuditEntry>(ConfigureEntity);
}
