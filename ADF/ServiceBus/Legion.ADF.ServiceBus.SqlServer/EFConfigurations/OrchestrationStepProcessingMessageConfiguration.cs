using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class OrchestrationStepProcessingMessageConfiguration : IEntityTypeConfiguration<ServiceBus.Model.OrchestrationStepProcessingMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessingMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.OrchestrationStepProcessingMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.OrchestrationStepProcessingMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessingMessage);

		entityBuilder.ToTable("OrchestrationStepProcessingMessage", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepProcessing, "IXFK_OrchestrationStepProcessingMessage_OrchStepProcessing");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepProcessingMessageType, "IXFK_OrchestrationStepProcessingMessage_OrchStepProcessingMessageType");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingMessage)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOrchestrationStepProcessing).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.HasOne(d => d.OrchestrationStepProcessing)
			.WithMany(p => p.OrchestrationStepProcessingMessages)
			.HasForeignKey(d => d.IdOrchestrationStepProcessing)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingMessage_IdOrchStepProcessing");

		entityBuilder.HasOne(d => d.OrchestrationStepProcessingMessageType)
			.WithMany(p => p.OrchestrationStepProcessingMessages)
			.HasForeignKey(d => d.IdOrchestrationStepProcessingMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingMessage_IdOrchStepProcessingMessageType");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.OrchestrationStepProcessingMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
