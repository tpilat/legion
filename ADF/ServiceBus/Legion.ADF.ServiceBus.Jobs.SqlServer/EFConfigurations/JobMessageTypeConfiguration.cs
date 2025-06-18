using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

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
		modelBuilder.Entity<Jobs.Model.JobMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
