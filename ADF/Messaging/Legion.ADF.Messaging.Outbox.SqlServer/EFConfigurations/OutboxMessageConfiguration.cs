using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxMessage);

		entityBuilder.ToTable("OutboxMessage", "outbox");

		entityBuilder.HasIndex(e => e.IdMessageContent, "IXFK_OutboxMessage_OutboxMessageContent");

		entityBuilder.HasIndex(e => e.IdMessageContent, "UQ_OutboxMessage_IdMessageContent")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_OutboxMessage_MessageType");

		entityBuilder.HasIndex(e => e.IdOutboxInstance, "IXFK_OutboxMessage_OutboxInstance");

		entityBuilder.HasIndex(e => e.IdOutboxMessageStatus, "IXFK_OutboxMessage_OutboxMessageStatus");

		entityBuilder.HasIndex(e => e.IdOutboxQueue, "IXFK_OutboxMessage_OutboxQueue");

		entityBuilder.Property(e => e.IdOutboxMessage)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxMessageStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MessageId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.BusinessId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CorrelationId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.SessionId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Publisher)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.PublisherId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.TargetTopic)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.TargetQueueName)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.HasOne(d => d.MessageContent)
			.WithOne(p => p.OutboxMessage)
			.HasForeignKey<Outbox.Model.OutboxMessage>(d => d.IdMessageContent)
			.HasConstraintName("FK_OutboxMessage_IdMessageContent");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.OutboxMessages)
			.HasForeignKey(d => d.IdMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessage_IdMessageType");

		entityBuilder.HasOne(d => d.OutboxInstance)
			.WithMany(p => p.OutboxMessages)
			.HasForeignKey(d => d.IdOutboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessage_IdOutboxInstance");

		entityBuilder.HasOne(d => d.OutboxMessageStatus)
			.WithMany(p => p.OutboxMessages)
			.HasForeignKey(d => d.IdOutboxMessageStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessage_IdOutboxMessageStatus");

		entityBuilder.HasOne(d => d.OutboxQueue)
			.WithMany(p => p.OutboxMessages)
			.HasForeignKey(d => d.IdOutboxQueue)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessage_IdOutboxQueue");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
