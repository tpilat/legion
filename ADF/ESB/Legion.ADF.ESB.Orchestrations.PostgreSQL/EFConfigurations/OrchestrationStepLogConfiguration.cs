using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class OrchestrationStepLogConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepLog);

		entityBuilder.ToTable("OrchestrationStepLog", "orch");

		entityBuilder.HasIndex(e => e.IdMessageProcessingLog, "IXFK_OrchestrationStepLog_IdMessageProcessingLog");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepInstance, "IXFK_OrchestrationStepLog_IdOrchestrationStepInstance");

		entityBuilder.HasIndex(e => e.IdStepStatus, "IXFK_OrchestrationStepLog_IdStepStatus");

		entityBuilder.Property(e => e.IdOrchestrationStepLog).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.Detail).IsRequired();

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.OrchestrationStepInstance)
			.WithMany(p => p.OrchestrationStepLogs)
			.HasForeignKey(d => d.IdOrchestrationStepInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepLog_IdOrchestrationStepInstance");

		entityBuilder.HasOne(d => d.StepStatus)
			.WithMany(p => p.OrchestrationStepLogs)
			.HasForeignKey(d => d.IdStepStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepLog_IdStepStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepLog>(ConfigureEntity);

		return modelBuilder;
	}
}
