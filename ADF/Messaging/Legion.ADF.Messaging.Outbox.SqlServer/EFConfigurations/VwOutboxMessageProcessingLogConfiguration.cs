using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class VwOutboxMessageProcessingLogConfiguration : IEntityTypeConfiguration<Outbox.Model.VwOutboxMessageProcessingLog>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwOutboxMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwOutboxMessageProcessingLog> entityBuilder)
	{
		entityBuilder.ToView("VwOutboxMessageProcessingLog", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOutboxMessageProcessingLog).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdOutboxMessageStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.OutboxMessageStatusCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.OutboxMessageStatusName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Outbox.Model.VwOutboxMessageProcessingLog>(ConfigureEntity);
}
