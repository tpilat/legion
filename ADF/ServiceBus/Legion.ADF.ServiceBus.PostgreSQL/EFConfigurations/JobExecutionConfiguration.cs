using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class JobExecutionConfiguration : IEntityTypeConfiguration<ServiceBus.Model.JobExecution>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobExecution\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.JobExecution> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.JobExecution> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobExecution);

		entityBuilder.ToTable("JobExecution", "jobs");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobExecution_Job");

		entityBuilder.HasIndex(e => e.IdJobStatus, "IXFK_JobExecution_JobStatus");

		entityBuilder.HasIndex(e => e.StatisticsStartHourUtc, "IX_JobExecution_StatisticsStartHourUtc");

		entityBuilder.Property(e => e.IdJobExecution)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.StartUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.EndUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdJobStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.StatisticsStartHourUtc).HasColumnType("timestamptz");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobExecutions)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobExecution_IdJob");

		entityBuilder.HasOne(d => d.JobStatus)
			.WithMany(p => p.JobExecutions)
			.HasForeignKey(d => d.IdJobStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobExecution_IdJobStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.JobExecution>(ConfigureEntity);

		return modelBuilder;
	}
}
