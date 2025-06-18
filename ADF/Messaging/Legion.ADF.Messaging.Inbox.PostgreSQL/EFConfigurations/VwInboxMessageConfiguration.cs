using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class VwInboxMessageConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxMessage>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxMessage> entityBuilder)
	{
		entityBuilder.ToView("VwInboxMessage", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.InboxMessageStatusCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.InboxMessageStatusName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.MessageId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.BusinessId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.SessionId).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Publisher).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.TargetTopic).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.TargetQueueName).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uuid");

		entityBuilder.Property(e => e.MessageTypeCode)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.MessageTypeName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.MessageTypeNamespace)
			.IsRequired()
			.HasColumnType("varchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxMessage>(ConfigureEntity);
}
