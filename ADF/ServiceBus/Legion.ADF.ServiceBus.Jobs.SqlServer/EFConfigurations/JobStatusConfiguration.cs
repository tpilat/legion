using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

public class JobStatusConfiguration : IEntityTypeConfiguration<Jobs.Model.JobStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Jobs.Model.JobStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Jobs.Model.JobStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobStatus);

		entityBuilder.ToTable("JobStatus", "jobs");

		entityBuilder.Property(e => e.IdJobStatus)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Jobs.Model.JobStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
