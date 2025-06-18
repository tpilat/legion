using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class VwOrchestrationConfiguration : IEntityTypeConfiguration<Orchestrations.Model.VwOrchestration>
{
	public void Configure(EntityTypeBuilder<Orchestrations.Model.VwOrchestration> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.VwOrchestration> entityBuilder)
	{
		entityBuilder.ToView("VwOrchestration", "orch")
			.HasNoKey();

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ValidTo).HasColumnType("timestamp(6)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Orchestrations.Model.VwOrchestration>(ConfigureEntity);
}
