using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class JobLogConfiguration : IEntityTypeConfiguration<Components.Model.JobLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.JobLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.JobLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobLog);

		entityBuilder.ToTable("JobLog", "comp");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobLog_IdJob");

		entityBuilder.HasIndex(e => e.IdJobStatus, "IXFK_JobLog_IdJobStatus");

		entityBuilder.HasIndex(e => e.IdMessageProcessingLog, "IXFK_JobLog_IdMessageProcessingLog");

		entityBuilder.Property(e => e.IdJobLog).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.Detail).IsRequired();

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

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
		modelBuilder.Entity<Components.Model.JobLog>(ConfigureEntity);

		return modelBuilder;
	}
}
