using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auditing.PostgreSQL.Audit;

public class VwApplicationEntryTokenConfiguration : IEntityTypeConfiguration<Auditing.Audit.VwApplicationEntryToken>
{
	public void Configure(EntityTypeBuilder<Auditing.Audit.VwApplicationEntryToken> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auditing.Audit.VwApplicationEntryToken> entityBuilder)
	{
		entityBuilder.ToView("VwApplicationEntryToken", "aud")
			.HasNoKey();

		entityBuilder.Property(e => e.Token).IsRequired();

		entityBuilder.Property(e => e.SourceFilePath).IsRequired();
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Auditing.Audit.VwApplicationEntryToken>(ConfigureEntity);
}
