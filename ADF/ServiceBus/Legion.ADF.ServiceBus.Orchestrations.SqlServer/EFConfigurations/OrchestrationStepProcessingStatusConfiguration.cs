using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Orchestrations.SqlServer;

public class OrchestrationStepProcessingStatusConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepProcessingStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessingStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessingStatus);

		entityBuilder.ToTable("OrchestrationStepProcessingStatus", "orch");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingStatus)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepProcessingStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
