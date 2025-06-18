using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Config.PostgreSQL;

public class ConfigurationClassConfiguration : IEntityTypeConfiguration<Config.Model.ConfigurationClass>
{
	public const string PrimaryKeyFormatter = "{{\"IdConfigurationClass\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Config.Model.ConfigurationClass> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Config.Model.ConfigurationClass> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdConfigurationClass);

		entityBuilder.ToTable("ConfigurationClass", "conf");

		entityBuilder.HasIndex(e => e.RootPath, "UQ_ConfigurationClass_RootPath")
				.IsUnique();

		entityBuilder.Property(e => e.IdConfigurationClass)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.RootPath).IsRequired();

		entityBuilder.Property(e => e.DisplayName)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Config.Model.ConfigurationClass>(ConfigureEntity);

		return modelBuilder;
	}
}
