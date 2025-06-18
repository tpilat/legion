using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Config.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.RootPath)
			.IsRequired()
			.HasColumnType("nvarchar(4000)")
			.HasMaxLength(4000);

		entityBuilder.Property(e => e.DisplayName)
			.IsRequired()
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Class).HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Config.Model.ConfigurationClass>(ConfigureEntity);

		return modelBuilder;
	}
}
