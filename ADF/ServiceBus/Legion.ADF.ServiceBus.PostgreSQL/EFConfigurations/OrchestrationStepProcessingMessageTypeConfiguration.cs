using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class OrchestrationStepProcessingMessageTypeConfiguration : IEntityTypeConfiguration<ServiceBus.Model.OrchestrationStepProcessingMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessingMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.OrchestrationStepProcessingMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.OrchestrationStepProcessingMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessingMessageType);

		entityBuilder.ToTable("OrchestrationStepProcessingMessageType", "orch");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingMessageType)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.OrchestrationStepProcessingMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
