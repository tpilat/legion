using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class VwJobConfiguration : IEntityTypeConfiguration<ServiceBus.Model.VwJob>
{
	public void Configure(EntityTypeBuilder<ServiceBus.Model.VwJob> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.VwJob> entityBuilder)
	{
		entityBuilder.ToView("VwJob", "jobs")
			.HasNoKey();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)");

		entityBuilder.Property(e => e.Description).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.IdJobRunType).HasColumnType("uuid");

		entityBuilder.Property(e => e.JobRunType)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.IdJobStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.JobStatus)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.CronExpression).HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.NextProcessinUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdDefaultHost).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<ServiceBus.Model.VwJob>(ConfigureEntity);
}
