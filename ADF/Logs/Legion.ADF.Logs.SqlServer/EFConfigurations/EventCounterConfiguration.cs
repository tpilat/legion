using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

public class EventCounterConfiguration : IEntityTypeConfiguration<Logs.Model.EventCounter>
{
	public const string PrimaryKeyFormatter = "{{\"IdEventCounter\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.EventCounter> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.EventCounter> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdEventCounter);

		entityBuilder.ToTable("EventCounter", "log");

		entityBuilder.HasIndex(e => e.IdEventCounterCategory, "IXFK_EventCounter_EventCounterCategory");

		entityBuilder.Property(e => e.IdEventCounter)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdEventCounterCategory).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.DisplayName)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.CounterType)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.DisplayRateTimeScale)
			.HasColumnType("nvarchar(31)")
			.HasMaxLength(31);

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.DisplayUnits)
			.HasColumnType("nvarchar(31)")
			.HasMaxLength(31);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.EventCounter>(ConfigureEntity);

		return modelBuilder;
	}
}
