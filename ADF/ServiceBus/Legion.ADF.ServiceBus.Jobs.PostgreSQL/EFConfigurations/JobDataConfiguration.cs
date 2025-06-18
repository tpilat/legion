using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public class JobDataConfiguration : IEntityTypeConfiguration<Jobs.Model.JobData>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobData\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Jobs.Model.JobData> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Jobs.Model.JobData> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobData);

		entityBuilder.ToTable("JobData", "jobs");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobData_Job");

		entityBuilder.Property(e => e.IdJobData)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.JobDataIdentifier)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastModifiedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.MimeType)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.ContentEncoding)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("bytea");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Name)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobDatas)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobData_IdJob");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Jobs.Model.JobData>(ConfigureEntity);

		return modelBuilder;
	}
}
