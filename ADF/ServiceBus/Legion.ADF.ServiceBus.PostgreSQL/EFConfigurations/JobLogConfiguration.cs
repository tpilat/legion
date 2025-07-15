using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class JobLogConfiguration : IEntityTypeConfiguration<ServiceBus.Model.JobLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.JobLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.JobLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobLog);

		entityBuilder.ToTable("JobLog", "jobs");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobLog_Job");

		entityBuilder.HasIndex(e => e.IdJobExecution, "IXFK_JobLog_JobExecution");

		entityBuilder.HasIndex(e => e.IdJobStatus, "IXFK_JobLog_JobStatus");

		entityBuilder.Property(e => e.IdJobLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdJobStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.IdMessageProcessingLog).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdJobExecution).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobLogs)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobLog_IdJob");

		entityBuilder.HasOne(d => d.JobExecution)
			.WithMany(p => p.JobLogs)
			.HasForeignKey(d => d.IdJobExecution)
			.HasConstraintName("FK_JobLog_IdJobExecution");

		entityBuilder.HasOne(d => d.JobStatus)
			.WithMany(p => p.JobLogs)
			.HasForeignKey(d => d.IdJobStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobLog_IdJobStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.JobLog>(ConfigureEntity);

		return modelBuilder;
	}
}
