using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class InboxMessageArchiveConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxMessageArchive>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxMessageArchive> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxMessageArchive> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxMessage);

		entityBuilder.ToTable("InboxMessageArchive", "inbox");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_InboxMessageArchive_InboxInstance");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_InboxMessageArchive_Table1");

		entityBuilder.HasIndex(e => e.IdInboxMessageStatus, "IXFK_InboxMessageArchive_InboxMessageStatus");

		entityBuilder.HasIndex(e => e.IdInboxQueue, "IXFK_InboxMessageArchive_InboxQueue");

		entityBuilder.HasIndex(e => e.IdMessageContent, "IXFK_InboxMessageArchive_InboxMessageContent");

		entityBuilder.HasIndex(e => e.IdMessageContent, "UQ_InboxMessageArchive_IdMessageContent")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_InboxMessageArchive_MessageType");

		entityBuilder.Property(e => e.IdInboxMessage)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdInboxMessageStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uniqueidentifier");

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

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.HasOne(d => d.InboxInstance)
			.WithMany(p => p.InboxMessageArchives)
			.HasForeignKey(d => d.IdInboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageArchive_IdInboxInstance");

		entityBuilder.HasOne(d => d.InboxMessageStatus)
			.WithMany(p => p.InboxMessageArchives)
			.HasForeignKey(d => d.IdInboxMessageStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageArchive_IdInboxMessageStatus");

		entityBuilder.HasOne(d => d.InboxQueue)
			.WithMany(p => p.InboxMessageArchives)
			.HasForeignKey(d => d.IdInboxQueue)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageArchive_IdInboxQueue");

		entityBuilder.HasOne(d => d.MessageContent)
			.WithOne(p => p.InboxMessageArchive)
			.HasForeignKey<Inbox.Model.InboxMessageArchive>(d => d.IdMessageContent)
			.HasConstraintName("FK_InboxMessageArchive_IdMessageContent");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.InboxMessageArchives)
			.HasForeignKey(d => d.IdMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageArchive_IdMessageType");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxMessageArchive>(ConfigureEntity);

		return modelBuilder;
	}
}
