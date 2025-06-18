using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class OrchestrationStepInstanceConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepInstance>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepInstance\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepInstance> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepInstance> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepInstance);

		entityBuilder.ToTable("OrchestrationStepInstance", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestration, "IXFK_OrchestrationStepInstance_IdOrchestration");

		entityBuilder.HasIndex(e => e.IdOrchestrationStep, "IXFK_OrchestrationStepInstance_IdOrchestrationStep");

		entityBuilder.HasIndex(e => e.IdStepStatus, "IXFK_OrchestrationStepInstance_IdStepStatus");

		entityBuilder.Property(e => e.IdOrchestrationStepInstance).ValueGeneratedNever();

		entityBuilder.Property(e => e.LastProcessedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.SucceededUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("timestamp(6)");

		entityBuilder.HasOne(d => d.Orchestration)
			.WithMany(p => p.OrchestrationStepInstances)
			.HasForeignKey(d => d.IdOrchestration)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepInstance_IdOrchestration");

		entityBuilder.HasOne(d => d.OrchestrationStep)
			.WithMany(p => p.OrchestrationStepInstances)
			.HasForeignKey(d => d.IdOrchestrationStep)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepInstance_IdOrchestrationStep");

		entityBuilder.HasOne(d => d.StepStatus)
			.WithMany(p => p.OrchestrationStepInstances)
			.HasForeignKey(d => d.IdStepStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepInstance_IdStepStatus");

		entityBuilder.HasMany(d => d.OrchestrationStepLogs)
			.WithOne()
			.HasForeignKey(d => d.IdMessageProcessingLog)
			.HasConstraintName("FK_OrchestrationStepLog_IdMessageProcessingLog");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
