using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.SqlServer;

public class ApplicationEntryConfiguration : IEntityTypeConfiguration<Audit.Model.ApplicationEntry>
{
	public const string PrimaryKeyFormatter = "{{\"IdApplicationEntry\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Audit.Model.ApplicationEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.ApplicationEntry> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdApplicationEntry);

		entityBuilder.ToTable("ApplicationEntry", "aud");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_ApplicationEntry_CorrelationId");

		entityBuilder.HasIndex(e => e.IdApplicationEntryToken, "IXFK_ApplicationEntry_ApplicationEntryToken");

		entityBuilder.HasIndex(e => e.IdAuditOperation, "IXFK_ApplicationEntry_AuditOperation");

		entityBuilder.Property(e => e.IdApplicationEntry)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdApplicationEntryToken).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdAuditOperation).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ExternalCorrelationId)
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.AggregateIdentifier)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.HttpMethod)
			.HasColumnType("nvarchar(15)")
			.HasMaxLength(15);

		entityBuilder.Property(e => e.Uri)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.RemoteIP)
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.HasOne(d => d.ApplicationEntryToken)
			.WithMany(p => p.ApplicationEntries)
			.HasForeignKey(d => d.IdApplicationEntryToken)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_ApplicationEntry_IdApplicationEntryToken");

		entityBuilder.HasOne(d => d.AuditOperation)
			.WithMany(p => p.ApplicationEntries)
			.HasForeignKey(d => d.IdAuditOperation)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_ApplicationEntry_IdAuditOperation");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Audit.Model.ApplicationEntry>(ConfigureEntity);

		return modelBuilder;
	}
}
