using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Config.PostgreSQL;

public class ConfigurationKeyValueConfiguration : IEntityTypeConfiguration<Config.Model.ConfigurationKeyValue>
{
	public const string PrimaryKeyFormatter = "{{\"IdConfigurationKeyValue\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Config.Model.ConfigurationKeyValue> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Config.Model.ConfigurationKeyValue> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdConfigurationKeyValue);

		entityBuilder.ToTable("ConfigurationKeyValue", "conf");

		entityBuilder.HasIndex(e => e.Key, "IX_ConfigurationKeyValue_Key");

		entityBuilder.HasIndex(e => e.Key, "UQ_ConfigurationKeyValue_Key")
				.IsUnique();

		entityBuilder.Property(e => e.IdConfigurationKeyValue)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Key).IsRequired();

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uuid")
				.IsConcurrencyToken();
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Config.Model.ConfigurationKeyValue>(ConfigureEntity);

		return modelBuilder;
	}
}
