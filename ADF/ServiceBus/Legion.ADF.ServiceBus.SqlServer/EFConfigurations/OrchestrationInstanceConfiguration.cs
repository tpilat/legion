using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class OrchestrationInstanceConfiguration : IEntityTypeConfiguration<ServiceBus.Model.OrchestrationInstance>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationInstance\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.OrchestrationInstance> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.OrchestrationInstance> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationInstance);

		entityBuilder.ToTable("OrchestrationInstance", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestration, "IXFK_OrchestrationInstance_Orchestration");

		entityBuilder.HasIndex(e => e.IdOrchestrationStatus, "IXFK_OrchestrationInstance_OrchestrationStatus");

		entityBuilder.Property(e => e.IdOrchestrationInstance)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOrchestrationStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.HasOne(d => d.Orchestration)
			.WithMany(p => p.OrchestrationInstances)
			.HasForeignKey(d => d.IdOrchestration)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationInstance_IdOrchestration");

		entityBuilder.HasOne(d => d.OrchestrationStatus)
			.WithMany(p => p.OrchestrationInstances)
			.HasForeignKey(d => d.IdOrchestrationStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationInstance_IdOrchestrationStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.OrchestrationInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
