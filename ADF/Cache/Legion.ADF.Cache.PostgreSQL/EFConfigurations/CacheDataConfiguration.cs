using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Cache.PostgreSQL;

public class CacheDataConfiguration : IEntityTypeConfiguration<Cache.Model.CacheData>
{
	public const string PrimaryKeyFormatter = "{{\"KeyHash\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Cache.Model.CacheData> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Cache.Model.CacheData> entityBuilder)
	{
		entityBuilder.HasKey(e => e.KeyHash);

		entityBuilder.ToTable("CacheData", "cache");

		entityBuilder.HasIndex(e => e.ExpiresUtc, "IX_CacheData_ExpiresUtc");

		entityBuilder.HasIndex(e => e.KeyPrefix450, "IX_CacheData_KeyPrefix");

		entityBuilder.HasIndex(e => e.ValueHash, "IX_CacheData_ValueHash");

		entityBuilder.Property(e => e.KeyHash).ValueGeneratedNever();

		entityBuilder.Property(e => e.ValueHash).IsRequired();

		entityBuilder.Property(e => e.Key).IsRequired();

		entityBuilder.Property(e => e.Value).IsRequired();

		entityBuilder.Property(e => e.KeyPrefix450).IsRequired();

		entityBuilder.Property(e => e.ExpiresUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.SlidingTime).HasColumnType("interval");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastAccessedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.RowVersion)
			.HasColumnType("uuid")
				.IsConcurrencyToken();
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Cache.Model.CacheData>(ConfigureEntity);

		return modelBuilder;
	}
}
