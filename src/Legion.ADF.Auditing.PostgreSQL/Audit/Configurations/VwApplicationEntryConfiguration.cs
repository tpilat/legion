using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auditing.PostgreSQL.Audit;

public class VwApplicationEntryConfiguration : IEntityTypeConfiguration<Auditing.Audit.VwApplicationEntry>
{
	public void Configure(EntityTypeBuilder<Auditing.Audit.VwApplicationEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auditing.Audit.VwApplicationEntry> entityBuilder)
	{
		entityBuilder.ToView("VwApplicationEntry", "aud")
			.HasNoKey();

		entityBuilder.Property(e => e.Token).IsRequired();

		entityBuilder.Property(e => e.SourceFilePath).IsRequired();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.CreatedAt).HasColumnType("timestamp(6)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Auditing.Audit.VwApplicationEntry>(ConfigureEntity);
}
