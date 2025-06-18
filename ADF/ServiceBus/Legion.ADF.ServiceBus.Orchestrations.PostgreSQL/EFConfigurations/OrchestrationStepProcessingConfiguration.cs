using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public class OrchestrationStepProcessingConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepProcessing>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessing\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessing> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessing> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessing);

		entityBuilder.ToTable("OrchestrationStepProcessing", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestrationInstance, "IXFK_OrchestrationStepProcessing_OrchestrationInstance");

		entityBuilder.HasIndex(e => e.IdOrchestrationStep, "IXFK_OrchestrationStepProcessing_OrchestrationStep");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepProcessingStatus, "IXFK_OrchestrationStepProcessing_OrchestrationStepStatus");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessing)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOrchestrationStep).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestrationInstance).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.HasOne(d => d.OrchestrationInstance)
			.WithMany(p => p.OrchestrationStepProcessings)
			.HasForeignKey(d => d.IdOrchestrationInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessing_IdOrchestrationInstance");

		entityBuilder.HasOne(d => d.OrchestrationStep)
			.WithMany(p => p.OrchestrationStepProcessings)
			.HasForeignKey(d => d.IdOrchestrationStep)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessing_IdOrchestrationStep");

		entityBuilder.HasOne(d => d.OrchestrationStepProcessingStatus)
			.WithMany(p => p.OrchestrationStepProcessings)
			.HasForeignKey(d => d.IdOrchestrationStepProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessing_IdOrchestrationStepProcessingSta");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepProcessing>(ConfigureEntity);

		return modelBuilder;
	}
}
