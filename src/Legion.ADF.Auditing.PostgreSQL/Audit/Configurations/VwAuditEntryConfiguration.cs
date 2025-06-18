using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auditing.PostgreSQL.Audit;

public class VwAuditEntryConfiguration : IEntityTypeConfiguration<Auditing.Audit.VwAuditEntry>
{
	public void Configure(EntityTypeBuilder<Auditing.Audit.VwAuditEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auditing.Audit.VwAuditEntry> entityBuilder)
	{
		entityBuilder.ToView("VwAuditEntry", "aud")
			.HasNoKey();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.CreatedAt).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.AuditType).IsRequired();

		entityBuilder.Property(e => e.TableName).IsRequired();
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Auditing.Audit.VwAuditEntry>(ConfigureEntity);
}
