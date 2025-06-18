using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwQueuedMessageConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwQueuedMessage>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwQueuedMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwQueuedMessage> entityBuilder)
	{
		entityBuilder.ToView("VwQueuedMessage", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdQueuedMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.MessageProcessingStatusCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.MessageProcessingStatusName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.AssignedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.MessageStatusCode).HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.MessageStatusName).HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdQueueMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdTopicMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.MessageId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.BusinessId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.SessionId).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Publisher).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.MessageTypeCode).HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.MessageTypeName).HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.MessageTypeNamespace).HasColumnType("varchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwQueuedMessage>(ConfigureEntity);
}
