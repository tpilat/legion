using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class JobStatisticsConfiguration : IEntityTypeConfiguration<ServiceBus.Model.JobStatistics>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobStatistics\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.JobStatistics> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.JobStatistics> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobStatistics);

		entityBuilder.ToTable("JobStatistics", "jobs");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobStatistics_Job");

		entityBuilder.HasIndex(e => new { e.IdJob, e.StartHourUtc }, "UQ_JobStatistics_IdJob_StartHour")
				.IsUnique();

		entityBuilder.Property(e => e.IdJobStatistics)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.StartHourUtc).HasColumnType("timestamptz");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobStatistics)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobStatistics_IdJob");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.JobStatistics>(ConfigureEntity);

		return modelBuilder;
	}
}
