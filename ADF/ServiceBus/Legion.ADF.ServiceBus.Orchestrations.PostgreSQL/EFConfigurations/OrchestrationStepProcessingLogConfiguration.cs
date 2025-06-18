using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public class OrchestrationStepProcessingLogConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStepProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStepProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStepProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStepProcessingLog);

		entityBuilder.ToTable("OrchestrationStepProcessingLog", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepProcessing, "IXFK_OrchestrationStepProcessingLog_OrchStepProcessing");

		entityBuilder.HasIndex(e => e.IdOrchestrationStepProcessingStatus, "IXFK_OrchestrationStepProcessingLog_OrchStepProcessingStatus");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOrchestrationStepProcessing).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdOrchestrationStepProcessingStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.IdMessageProcessingLog).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.OrchestrationStepProcessing)
			.WithMany(p => p.OrchestrationStepProcessingLogs)
			.HasForeignKey(d => d.IdOrchestrationStepProcessing)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingLog_IdOrchStepProcessing");

		entityBuilder.HasOne(d => d.OrchestrationStepProcessingStatus)
			.WithMany(p => p.OrchestrationStepProcessingLogs)
			.HasForeignKey(d => d.IdOrchestrationStepProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStepProcessingLog_IdOrchStepProcessingStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStepProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
