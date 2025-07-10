using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class VwOrchestrationConfiguration : IEntityTypeConfiguration<ServiceBus.Model.VwOrchestration>
{
	public void Configure(EntityTypeBuilder<ServiceBus.Model.VwOrchestration> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.VwOrchestration> entityBuilder)
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
		=> modelBuilder.Entity<ServiceBus.Model.VwOrchestration>(ConfigureEntity);
}
