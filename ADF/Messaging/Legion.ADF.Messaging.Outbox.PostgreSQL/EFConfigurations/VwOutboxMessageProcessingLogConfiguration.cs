using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class VwOutboxMessageProcessingLogConfiguration : IEntityTypeConfiguration<Outbox.Model.VwOutboxMessageProcessingLog>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwOutboxMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwOutboxMessageProcessingLog> entityBuilder)
	{
		entityBuilder.ToView("VwOutboxMessageProcessingLog", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOutboxMessageProcessingLog).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOutboxMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdOutboxMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.OutboxMessageStatusCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.OutboxMessageStatusName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Outbox.Model.VwOutboxMessageProcessingLog>(ConfigureEntity);
}
