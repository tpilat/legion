using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class VwOutboxMessageConfiguration : IEntityTypeConfiguration<Outbox.Model.VwOutboxMessage>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwOutboxMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwOutboxMessage> entityBuilder)
	{
		entityBuilder.ToView("VwOutboxMessage", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOutboxMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxMessageStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.OutboxMessageStatusCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.OutboxMessageStatusName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MessageId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.BusinessId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.SessionId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Publisher).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.TargetTopic).HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.TargetQueueName).HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MessageTypeCode)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeNamespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Outbox.Model.VwOutboxMessage>(ConfigureEntity);
}
