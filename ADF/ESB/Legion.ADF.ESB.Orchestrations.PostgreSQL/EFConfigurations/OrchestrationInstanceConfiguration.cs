using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class OrchestrationInstanceConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationInstance>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationInstance\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationInstance> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationInstance> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationInstance);

		entityBuilder.ToTable("OrchestrationInstance", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestration, "IXFK_OrchestrationInstance_IdOrchestration");

		entityBuilder.HasIndex(e => e.IdOrchestrationStatus, "IXFK_OrchestrationInstance_IdOrchestrationStatus");

		entityBuilder.Property(e => e.IdOrchestrationInstance).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

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
		modelBuilder.Entity<Orchestrations.Model.OrchestrationInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
