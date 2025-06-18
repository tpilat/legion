using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

public class EventCounterDataConfiguration : IEntityTypeConfiguration<Logs.Model.EventCounterData>
{
	public const string PrimaryKeyFormatter = "{{\"IdEventCounterData\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.EventCounterData> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.EventCounterData> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdEventCounterData);

		entityBuilder.ToTable("EventCounterData", "log");

		entityBuilder.HasIndex(e => e.IdEventCounter, "IXFK_EventCounterData_EventCounter");

		entityBuilder.Property(e => e.IdEventCounterData)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdEventCounter).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.EventCounterData>(ConfigureEntity);

		return modelBuilder;
	}
}
