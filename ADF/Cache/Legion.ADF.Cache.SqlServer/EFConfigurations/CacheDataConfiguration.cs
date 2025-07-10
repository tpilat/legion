using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Cache.SqlServer;

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

		entityBuilder.Property(e => e.KeyHash)
			.HasColumnType("nvarchar(32)")
			.HasMaxLength(32)
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.ValueHash)
			.IsRequired()
			.HasColumnType("nvarchar(32)")
			.HasMaxLength(32);

		entityBuilder.Property(e => e.Key)
			.IsRequired()
			.HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Value)
			.IsRequired()
			.HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.KeyPrefix450)
			.IsRequired()
			.HasColumnType("nvarchar(450)")
			.HasMaxLength(450);

		entityBuilder.Property(e => e.ExpiresUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.SlidingTime).HasColumnType("time(7)");

		entityBuilder.Property(e => e.LastAccessedUtc).HasColumnType("datetime2(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Cache.Model.CacheData>(ConfigureEntity);

		return modelBuilder;
	}
}
