using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class VwJobConfiguration : IEntityTypeConfiguration<ServiceBus.Model.VwJob>
{
	public void Configure(EntityTypeBuilder<ServiceBus.Model.VwJob> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.VwJob> entityBuilder)
	{
		entityBuilder.ToView("VwJob", "jobs")
			.HasNoKey();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(255)");

		entityBuilder.Property(e => e.Description).HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IdJobRunType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.JobRunType)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.IdJobStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.JobStatus)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.CronExpression).HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.CronExpressionIncludeSeconds).HasColumnType("bit");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.NextProcessinUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdDefaultHost).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<ServiceBus.Model.VwJob>(ConfigureEntity);
}
