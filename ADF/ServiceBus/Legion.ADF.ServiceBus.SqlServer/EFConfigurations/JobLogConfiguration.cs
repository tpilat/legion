using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

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

		entityBuilder.HasIndex(e => e.IdJobStatus, "IXFK_JobLog_JobStatus");

		entityBuilder.Property(e => e.IdJobLog)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdJobStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdMessageProcessingLog).HasColumnType("uniqueidentifier");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobLogs)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobLog_IdJob");

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
