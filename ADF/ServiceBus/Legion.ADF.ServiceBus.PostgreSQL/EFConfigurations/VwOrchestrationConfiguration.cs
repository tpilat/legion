using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class VwOrchestrationConfiguration : IEntityTypeConfiguration<ServiceBus.Model.VwOrchestration>
{
	public void Configure(EntityTypeBuilder<ServiceBus.Model.VwOrchestration> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.VwOrchestration> entityBuilder)
	{
		entityBuilder.ToView("VwOrchestration", "orch")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");

		entityBuilder.Property(e => e.Name).HasColumnType("varchar(255)");

		entityBuilder.Property(e => e.Description).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.Namespace).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.Version).HasColumnType("varchar(31)");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<ServiceBus.Model.VwOrchestration>(ConfigureEntity);
}
