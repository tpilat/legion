using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class JobStatusConfiguration : IEntityTypeConfiguration<Components.Model.JobStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.JobStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.JobStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobStatus);

		entityBuilder.ToTable("JobStatus", "comp");

		entityBuilder.Property(e => e.IdJobStatus).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.HasMany(d => d.JobLogs)
			.WithOne()
			.HasForeignKey(d => d.IdMessageProcessingLog)
			.HasConstraintName("FK_JobLog_IdMessageProcessingLog");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.JobStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
