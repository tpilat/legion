using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class VwSubscribedMessageConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwSubscribedMessage>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwSubscribedMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwSubscribedMessage> entityBuilder)
	{
		entityBuilder.ToView("VwSubscribedMessage", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdSubscribedMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MessageProcessingStatusCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.MessageProcessingStatusName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.AssignedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MessageStatusCode).HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.MessageStatusName).HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdQueueMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopicMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.MessageId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.BusinessId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.SessionId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Publisher).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.MessageTypeCode).HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeName).HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeNamespace).HasColumnType("nvarchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwSubscribedMessage>(ConfigureEntity);
}
