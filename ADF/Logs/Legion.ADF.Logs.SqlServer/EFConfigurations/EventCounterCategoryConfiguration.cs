using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

public class EventCounterCategoryConfiguration : IEntityTypeConfiguration<Logs.Model.EventCounterCategory>
{
	public const string PrimaryKeyFormatter = "{{\"IdEventCounterCategory\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.EventCounterCategory> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.EventCounterCategory> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdEventCounterCategory);

		entityBuilder.ToTable("EventCounterCategory", "log");

		entityBuilder.Property(e => e.IdEventCounterCategory)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Source)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.DisplayName)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.EventCounterCategory>(ConfigureEntity);

		return modelBuilder;
	}
}
