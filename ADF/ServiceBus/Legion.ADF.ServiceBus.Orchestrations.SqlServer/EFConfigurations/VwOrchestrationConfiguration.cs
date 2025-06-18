using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Orchestrations.SqlServer;

public class VwOrchestrationConfiguration : IEntityTypeConfiguration<Orchestrations.Model.VwOrchestration>
{
	public void Configure(EntityTypeBuilder<Orchestrations.Model.VwOrchestration> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.VwOrchestration> entityBuilder)
	{
		entityBuilder.ToView("VwOrchestration", "orch")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(255)");

		entityBuilder.Property(e => e.Description).HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IsSingleton).HasColumnType("bit");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.Version)
			.IsRequired()
			.HasColumnType("nvarchar(31)");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Orchestrations.Model.VwOrchestration>(ConfigureEntity);
}
