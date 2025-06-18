using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class JobDataConfiguration : IEntityTypeConfiguration<Components.Model.JobData>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobData\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.JobData> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.JobData> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobData);

		entityBuilder.ToTable("JobData", "comp");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobData_IdJob");

		entityBuilder.Property(e => e.IdJobData).ValueGeneratedNever();

		entityBuilder.Property(e => e.Key)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.LastModifiedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.ContentEncoding).HasMaxLength(63);

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Name).HasMaxLength(511);

		entityBuilder.Property(e => e.RelaltivePath).HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobData)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobData_IdJob");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.JobData>(ConfigureEntity);

		return modelBuilder;
	}
}
