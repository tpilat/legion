using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Cache.PostgreSQL;

public class DistributedLockConfiguration : IEntityTypeConfiguration<Cache.Model.DistributedLock>
{
	public const string PrimaryKeyFormatter = "{{\"KeyHash\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Cache.Model.DistributedLock> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Cache.Model.DistributedLock> entityBuilder)
	{
		entityBuilder.HasKey(e => e.KeyHash);

		entityBuilder.ToTable("DistributedLock", "cache");

		entityBuilder.HasIndex(e => e.LockId, "IX_DistributedLock_LockId");

		entityBuilder.Property(e => e.KeyHash).ValueGeneratedNever();

		entityBuilder.Property(e => e.LockKey).IsRequired();

		entityBuilder.Property(e => e.LockId).IsRequired();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ExpiresUtc).HasColumnType("timestamptz");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Cache.Model.DistributedLock>(ConfigureEntity);

		return modelBuilder;
	}
}
