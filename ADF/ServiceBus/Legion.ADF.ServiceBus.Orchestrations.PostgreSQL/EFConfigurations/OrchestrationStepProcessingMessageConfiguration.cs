using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public class OrchestrationStepProcessingMessageConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepProcessingMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessingMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessingMessage);

		entityBuilder.ToTable("OrchestrationStepProcessingMessage", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepProcessing, "IXFK_OrchestrationStepProcessingMessage_OrchStepProcessing");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepProcessingMessageType, "IXFK_OrchestrationStepProcessingMessage_OrchStepProcessingMessa");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingMessage)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOrchestrationStepProcessing).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.HasOne(d => d.OrchestrationStepProcessing)
			.WithMany(p => p.OrchestrationStepProcessingMessages)
			.HasForeignKey(d => d.IdOrchestrationStepProcessing)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingMessage_IdOrchStepProcessing");

		entityBuilder.HasOne(d => d.OrchestrationStepProcessingMessageType)
			.WithMany(p => p.OrchestrationStepProcessingMessages)
			.HasForeignKey(d => d.IdOrchestrationStepProcessingMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingMessage_IdOrchStepProcessingMessa");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepProcessingMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
