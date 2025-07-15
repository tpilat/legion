using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class JobConfiguration : IEntityTypeConfiguration<ServiceBus.Model.Job>
{
	public const string PrimaryKeyFormatter = "{{\"IdJob\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.Job> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.Job> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJob);

		entityBuilder.ToTable("Job", "jobs");

		entityBuilder.HasIndex(e => e.IdJobRunType, "IXFK_Job_JobRunType");

		entityBuilder.HasIndex(e => e.Name, "UQ_Job_Name")
				.IsUnique();

		entityBuilder.Property(e => e.IdJob)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Description)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IdJobRunType).HasColumnType("uuid");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.CronExpression)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.IdDefaultHost).HasColumnType("uuid");

		entityBuilder.Property(e => e.RowVersion)
			.HasColumnType("uuid")
				.IsConcurrencyToken();

		entityBuilder.HasOne(d => d.JobRunType)
			.WithMany(p => p.Jobs)
			.HasForeignKey(d => d.IdJobRunType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Job_IdJobRunType");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.Job>(ConfigureEntity);

		return modelBuilder;
	}
}
