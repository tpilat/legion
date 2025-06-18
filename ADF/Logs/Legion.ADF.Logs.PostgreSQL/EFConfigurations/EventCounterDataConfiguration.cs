using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

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
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdEventCounter).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.EventCounter)
			.WithMany(p => p.EventCounterDatas)
			.HasForeignKey(d => d.IdEventCounter)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_EventCounterData_IdEventCounter");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.EventCounterData>(ConfigureEntity);

		return modelBuilder;
	}
}
