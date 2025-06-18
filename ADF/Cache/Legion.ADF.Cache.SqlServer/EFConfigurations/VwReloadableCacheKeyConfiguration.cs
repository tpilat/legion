using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Cache.SqlServer;

public class VwReloadableCacheKeyConfiguration : IEntityTypeConfiguration<Cache.Model.VwReloadableCacheKey>
{
	public void Configure(EntityTypeBuilder<Cache.Model.VwReloadableCacheKey> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Cache.Model.VwReloadableCacheKey> entityBuilder)
	{
		entityBuilder.ToView("VwReloadableCacheKey", "cache")
			.HasNoKey();

		entityBuilder.Property(e => e.IdReloadableCacheKey).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Key).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Tags)
			.HasConversion(
				v => Legion.Serializer.JsonSerializerHelper.Serialize(v, true),
				v => Legion.Serializer.JsonSerializerHelper.Deserialize<List<string>>(v, true),
				new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
					(c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
					c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c == null ? null! : c.ToList())
			)
			.HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ReloadAtUtc).HasColumnType("datetime2(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Cache.Model.VwReloadableCacheKey>(ConfigureEntity);
}
