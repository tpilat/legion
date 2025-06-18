using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class OrchestrationStepStatusConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepStatus);

		entityBuilder.ToTable("OrchestrationStepStatus", "orch");

		entityBuilder.Property(e => e.IdOrchestrationStepStatus).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.HasMany(d => d.OrchestrationStepLogs)
			.WithOne()
			.HasForeignKey(d => d.IdMessageProcessingLog)
			.HasConstraintName("FK_OrchestrationStepLog_IdMessageProcessingLog");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
