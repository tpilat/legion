using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class JobRunTypeConfiguration : IEntityTypeConfiguration<ServiceBus.Model.JobRunType>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobRunType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.JobRunType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.JobRunType> entityBuilder)
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
		modelBuilder.Entity<ServiceBus.Model.JobRunType>(ConfigureEntity);

		return modelBuilder;
	}
}
