using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Config.PostgreSQL;

public class VwConfigurationClassConfiguration : IEntityTypeConfiguration<Config.Model.VwConfigurationClass>
{
	public void Configure(EntityTypeBuilder<Config.Model.VwConfigurationClass> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Config.Model.VwConfigurationClass> entityBuilder)
	{
		entityBuilder.ToView("VwConfigurationClass", "conf")
			.HasNoKey();

		entityBuilder.Property(e => e.IdConfigurationClass).HasColumnType("uuid");

		entityBuilder.Property(e => e.DisplayName).HasColumnType("varchar(255)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Config.Model.VwConfigurationClass>(ConfigureEntity);
}
