using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public class JobRunTypeConfiguration : IEntityTypeConfiguration<Jobs.Model.JobRunType>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobRunType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Jobs.Model.JobRunType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Jobs.Model.JobRunType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobRunType);

		entityBuilder.ToTable("JobRunType", "jobs");

		entityBuilder.Property(e => e.IdJobRunType)
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
		modelBuilder.Entity<Jobs.Model.JobRunType>(ConfigureEntity);

		return modelBuilder;
	}
}
