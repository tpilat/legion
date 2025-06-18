using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Config.SqlServer;

public class VwConfigurationClassConfiguration : IEntityTypeConfiguration<Config.Model.VwConfigurationClass>
{
	public void Configure(EntityTypeBuilder<Config.Model.VwConfigurationClass> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Config.Model.VwConfigurationClass> entityBuilder)
	{
		entityBuilder.ToView("VwConfigurationClass", "conf")
			.HasNoKey();

		entityBuilder.Property(e => e.IdConfigurationClass).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.RootPath)
			.IsRequired()
			.HasColumnType("nvarchar(4000)");

		entityBuilder.Property(e => e.DisplayName)
			.IsRequired()
			.HasColumnType("nvarchar(255)");

		entityBuilder.Property(e => e.Class).HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Config.Model.VwConfigurationClass>(ConfigureEntity);
}
