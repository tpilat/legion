using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class JobStatusConfiguration : IEntityTypeConfiguration<ServiceBus.Model.JobStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.JobStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.JobStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobStatus);

		entityBuilder.ToTable("JobStatus", "jobs");

		entityBuilder.Property(e => e.IdJobStatus)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.JobStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
