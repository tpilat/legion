using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Orchestrations.SqlServer;

public class OrchestrationStepProcessingMessageTypeConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepProcessingMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessingMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessingMessageType);

		entityBuilder.ToTable("OrchestrationStepProcessingMessageType", "orch");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingMessageType)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepProcessingMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
