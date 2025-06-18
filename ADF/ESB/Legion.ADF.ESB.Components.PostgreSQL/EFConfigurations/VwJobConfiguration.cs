using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class VwJobConfiguration : IEntityTypeConfiguration<Components.Model.VwJob>
{
	public void Configure(EntityTypeBuilder<Components.Model.VwJob> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.VwJob> entityBuilder)
	{
		entityBuilder.ToView("VwJob", "comp")
			.HasNoKey();

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.LastExecutionUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.NextExecutionUtc).HasColumnType("timestamp(6)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Components.Model.VwJob>(ConfigureEntity);
}
