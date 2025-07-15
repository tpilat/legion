using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class JobActivityConfiguration : IEntityTypeConfiguration<ServiceBus.Model.JobActivity>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobActivity\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.JobActivity> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.JobActivity> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobActivity);

		entityBuilder.ToTable("JobActivity", "jobs");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobActivity_Job");

		entityBuilder.HasIndex(e => e.IdJob, "UQ_JobActivity_IdJob")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdJobStatus, "IXFK_JobActivity_JobStatus");

		entityBuilder.Property(e => e.IdJobActivity)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdJobStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdCurrentHost).HasColumnType("uuid");

		entityBuilder.Property(e => e.AttachedToCurrentHostUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastStatusChangedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingStartedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingFinishedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.DelayedToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.RowVersion)
			.HasColumnType("uuid")
				.IsConcurrencyToken();

		entityBuilder.HasOne(d => d.Job)
			.WithOne(p => p.JobActivity)
			.HasForeignKey<ServiceBus.Model.JobActivity>(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobActivity_IdJob");

		entityBuilder.HasOne(d => d.JobStatus)
			.WithMany(p => p.JobActivities)
			.HasForeignKey(d => d.IdJobStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobActivity_IdJobStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.JobActivity>(ConfigureEntity);

		return modelBuilder;
	}
}
