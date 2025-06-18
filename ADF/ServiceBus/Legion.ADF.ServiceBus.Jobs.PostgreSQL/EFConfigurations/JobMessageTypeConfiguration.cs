using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public class JobMessageTypeConfiguration : IEntityTypeConfiguration<Jobs.Model.JobMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Jobs.Model.JobMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Jobs.Model.JobMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobMessageType);

		entityBuilder.ToTable("JobMessageType", "jobs");

		entityBuilder.Property(e => e.IdJobMessageType)
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
		modelBuilder.Entity<Jobs.Model.JobMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
