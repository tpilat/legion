using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Cache.SqlServer;

public class DistributedLockConfiguration : IEntityTypeConfiguration<Cache.Model.DistributedLock>
{
	public const string PrimaryKeyFormatter = "{{\"KeyHash\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Cache.Model.DistributedLock> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Cache.Model.DistributedLock> entityBuilder)
	{
		entityBuilder.HasKey(e => e.KeyHash);

		entityBuilder.ToTable("DistributedLock", "cache");

		entityBuilder.Property(e => e.KeyHash)
			.HasColumnType("nvarchar(32)")
			.HasMaxLength(32)
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.LockKey)
			.IsRequired()
			.HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.LockId)
			.IsRequired()
			.HasColumnType("nvarchar(32)")
			.HasMaxLength(32);

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ExpiresUtc).HasColumnType("datetime2(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Cache.Model.DistributedLock>(ConfigureEntity);

		return modelBuilder;
	}
}
