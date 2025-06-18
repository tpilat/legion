using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class InboxMessageConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxMessage);

		entityBuilder.ToTable("InboxMessage", "inbox");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_InboxMessage_InboxInstance");

		entityBuilder.HasIndex(e => e.IdInboxMessageStatus, "IXFK_InboxMessage_InboxMessageStatus");

		entityBuilder.HasIndex(e => e.IdInboxQueue, "IXFK_InboxMessage_InboxQueue");

		entityBuilder.HasIndex(e => e.IdMessageContent, "IXFK_InboxMessage_InboxMessageContent");

		entityBuilder.HasIndex(e => e.IdMessageContent, "UQ_InboxMessage_IdMessageContent")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_InboxMessage_MessageType");

		entityBuilder.Property(e => e.IdInboxMessage)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.MessageId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.BusinessId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CorrelationId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.SessionId).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Publisher)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.PublisherId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.TargetTopic)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.TargetQueueName)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.InboxInstance)
			.WithMany(p => p.InboxMessages)
			.HasForeignKey(d => d.IdInboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessage_IdInboxInstance");

		entityBuilder.HasOne(d => d.InboxMessageStatus)
			.WithMany(p => p.InboxMessages)
			.HasForeignKey(d => d.IdInboxMessageStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessage_IdInboxMessageStatus");

		entityBuilder.HasOne(d => d.InboxQueue)
			.WithMany(p => p.InboxMessages)
			.HasForeignKey(d => d.IdInboxQueue)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessage_IdInboxQueue");

		entityBuilder.HasOne(d => d.MessageContent)
			.WithOne(p => p.InboxMessage)
			.HasForeignKey<Inbox.Model.InboxMessage>(d => d.IdMessageContent)
			.HasConstraintName("FK_InboxMessage_IdMessageContent");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.InboxMessages)
			.HasForeignKey(d => d.IdMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessage_IdMessageType");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
