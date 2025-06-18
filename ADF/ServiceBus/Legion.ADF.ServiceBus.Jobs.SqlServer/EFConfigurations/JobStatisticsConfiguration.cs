using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

public class JobStatisticsConfiguration : IEntityTypeConfiguration<Jobs.Model.JobStatistics>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobStatistics\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Jobs.Model.JobStatistics> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Jobs.Model.JobStatistics> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobStatistics);

		entityBuilder.ToTable("JobStatistics", "jobs");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobStatistics_Job");

		entityBuilder.Property(e => e.IdJobStatistics)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.StartHoutUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.AverageDuration).HasColumnType("decimal(18, 0)");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobStatistics)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobStatistics_IdJob");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Jobs.Model.JobStatistics>(ConfigureEntity);

		return modelBuilder;
	}
}
