using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class OutboxQueueProcessingModeConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxQueueProcessingMode>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxQueueProcessingMode\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxQueueProcessingMode> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxQueueProcessingMode> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxQueueProcessingMode);

		entityBuilder.ToTable("OutboxQueueProcessingMode", "outbox");

		entityBuilder.Property(e => e.IdOutboxQueueProcessingMode)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxQueueProcessingMode>(ConfigureEntity);

		return modelBuilder;
	}
}
