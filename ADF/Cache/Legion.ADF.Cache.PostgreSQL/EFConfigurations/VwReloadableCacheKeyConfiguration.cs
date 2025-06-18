using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Cache.PostgreSQL;

public class VwReloadableCacheKeyConfiguration : IEntityTypeConfiguration<Cache.Model.VwReloadableCacheKey>
{
	public void Configure(EntityTypeBuilder<Cache.Model.VwReloadableCacheKey> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Cache.Model.VwReloadableCacheKey> entityBuilder)
	{
		entityBuilder.ToView("VwReloadableCacheKey", "cache")
			.HasNoKey();

		entityBuilder.Property(e => e.IdReloadableCacheKey).HasColumnType("uuid");

		entityBuilder.Property(e => e.Tags).HasColumnType("text[]");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ReloadAtUtc).HasColumnType("timestamptz");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Cache.Model.VwReloadableCacheKey>(ConfigureEntity);
}
