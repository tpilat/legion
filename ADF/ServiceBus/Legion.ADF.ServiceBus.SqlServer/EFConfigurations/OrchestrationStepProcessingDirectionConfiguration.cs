using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class OrchestrationStepProcessingDirectionConfiguration : IEntityTypeConfiguration<ServiceBus.Model.OrchestrationStepProcessingDirection>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessingDirection\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.OrchestrationStepProcessingDirection> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.OrchestrationStepProcessingDirection> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessingDirection);

		entityBuilder.ToTable("OrchestrationStepProcessingDirection", "orch");

		entityBuilder.HasIndex(e => e.IdFromStep, "IXFK_OrchestrationStepProcessingDirection_IdFromStep");

		entityBuilder.HasIndex(e => e.IdToStep, "IXFK_OrchestrationStepProcessingDirection_IdToStep");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingDirection)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdFromStep).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdToStep).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.HasOne(d => d.FromStep)
			.WithMany(p => p.OrchestrationStepProcessingDirections)
			.HasForeignKey(d => d.IdFromStep)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingDirection_IdFromStep");

		entityBuilder.HasOne(d => d.ToStep)
			.WithMany(p => p.ToStepOrchestrationStepProcessingDirections)
			.HasForeignKey(d => d.IdToStep)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingDirection_IdToStep");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.OrchestrationStepProcessingDirection>(ConfigureEntity);

		return modelBuilder;
	}
}
