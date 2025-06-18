using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.SqlServer;

public class VwAuditEntryConfiguration : IEntityTypeConfiguration<Audit.Model.VwAuditEntry>
{
	public void Configure(EntityTypeBuilder<Audit.Model.VwAuditEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.VwAuditEntry> entityBuilder)
	{
		entityBuilder.ToView("VwAuditEntry", "aud")
			.HasNoKey();

		entityBuilder.Property(e => e.IdAuditEntry).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdAuditOperation).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TableName)
			.IsRequired()
			.HasColumnType("nvarchar(255)");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.PrimaryKey).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.OldValues).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.NewValues).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.AffectedColumns).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.AuditCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceFrame).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Audit.Model.VwAuditEntry>(ConfigureEntity);
}
