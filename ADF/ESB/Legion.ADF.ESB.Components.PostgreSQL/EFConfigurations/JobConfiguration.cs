using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class JobConfiguration : IEntityTypeConfiguration<Components.Model.Job>
{
	public const string PrimaryKeyFormatter = "{{\"IdJob\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.Job> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.Job> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJob);

		entityBuilder.ToTable("Job", "comp");

		entityBuilder.HasIndex(e => e.IdJobStatus, "IXFK_Job_IdJobStatus");

		entityBuilder.HasIndex(e => e.IdJobType, "IXFK_Job_IdJobType");

		entityBuilder.Property(e => e.IdJob).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Description).HasMaxLength(1023);

		entityBuilder.Property(e => e.Class)
			.IsRequired()
			.HasMaxLength(2047);

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.CronExpression).HasMaxLength(63);

		entityBuilder.Property(e => e.LastExecutionUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.NextExecutionUtc).HasColumnType("timestamp(6)");

		entityBuilder.HasOne(d => d.JobStatus)
			.WithMany(p => p.Jobs)
			.HasForeignKey(d => d.IdJobStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Job_IdJobStatus");

		entityBuilder.HasOne(d => d.JobType)
			.WithMany(p => p.Jobs)
			.HasForeignKey(d => d.IdJobType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Job_IdJobType");

		entityBuilder.HasMany(d => d.JobLogs)
			.WithOne()
			.HasForeignKey(d => d.IdMessageProcessingLog)
			.HasConstraintName("FK_JobLog_IdMessageProcessingLog");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.Job>(ConfigureEntity);

		return modelBuilder;
	}
}
