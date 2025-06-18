using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class JobTypeConfiguration : IEntityTypeConfiguration<Components.Model.JobType>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.JobType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.JobType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobType);

		entityBuilder.ToTable("JobType", "comp");

		entityBuilder.Property(e => e.IdJobType).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.JobType>(ConfigureEntity);

		return modelBuilder;
	}
}
