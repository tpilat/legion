using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

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
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdEventCounterCategory).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.DisplayName)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.CounterType)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.DisplayRateTimeScale)
			.HasColumnType("varchar(31)")
			.HasMaxLength(31);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.Property(e => e.DisplayUnits)
			.HasColumnType("varchar(31)")
			.HasMaxLength(31);

		entityBuilder.HasOne(d => d.EventCounterCategory)
			.WithMany(p => p.EventCounters)
			.HasForeignKey(d => d.IdEventCounterCategory)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_EventCounter_IdEventCounterCategory");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.EventCounter>(ConfigureEntity);

		return modelBuilder;
	}
}
