using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auditing.PostgreSQL.Audit;

public class ApplicationEntryConfiguration : IEntityTypeConfiguration<Auditing.Audit.ApplicationEntry>
{
	public const string PrimaryKeyFormatter = "{{\"IdApplicationEntry\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auditing.Audit.ApplicationEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auditing.Audit.ApplicationEntry> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdApplicationEntry);

		entityBuilder.ToTable("ApplicationEntry", "aud");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_ApplicationEntry_CorrelationId");

		entityBuilder.HasIndex(e => e.IdApplicationEntryToken, "IX_ApplicationEntry_Token");

		entityBuilder.Property(e => e.IdApplicationEntry).HasDefaultValueSql("uuid_generate_v4()");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.ExternalCorrelationId).HasMaxLength(127);

		entityBuilder.Property(e => e.MainEntityIdentifier).HasMaxLength(511);

		entityBuilder.Property(e => e.Uri).HasMaxLength(1023);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auditing.Audit.ApplicationEntry>(ConfigureEntity);

		return modelBuilder;
	}
}
