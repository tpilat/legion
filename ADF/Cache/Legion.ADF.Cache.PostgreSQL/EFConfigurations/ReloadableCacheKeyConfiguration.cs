using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Cache.PostgreSQL;

public class ReloadableCacheKeyConfiguration : IEntityTypeConfiguration<Cache.Model.ReloadableCacheKey>
{
	public const string PrimaryKeyFormatter = "{{\"IdReloadableCacheKey\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Cache.Model.ReloadableCacheKey> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Cache.Model.ReloadableCacheKey> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdReloadableCacheKey);

		entityBuilder.ToTable("ReloadableCacheKey", "cache");

		entityBuilder.Property(e => e.IdReloadableCacheKey)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Tags).HasColumnType("text[]");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ReloadAtUtc).HasColumnType("timestamptz");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Cache.Model.ReloadableCacheKey>(ConfigureEntity);

		return modelBuilder;
	}
}
