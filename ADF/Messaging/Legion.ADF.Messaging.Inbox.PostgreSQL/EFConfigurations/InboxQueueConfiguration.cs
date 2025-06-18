using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class InboxQueueConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxQueue>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxQueue\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxQueue> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxQueue> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxQueue);

		entityBuilder.ToTable("InboxQueue", "inbox");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_InboxQueue_InboxInstance");

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_InboxQueue_MessageType");

		entityBuilder.HasIndex(e => e.IdProcessingMode, "IXFK_InboxQueue_InboxQueueProcessingMode_02");

		entityBuilder.HasIndex(e => e.IdSuspendingMode, "IXFK_InboxQueue_InboxQueueProcessingMode");

		entityBuilder.HasIndex(e => e.Name, "UQ_InboxQueue_Name")
				.IsUnique();

		entityBuilder.HasIndex(e => e.ReceivedEventNamespace, "UQ_InboxQueue_ReceivedEventNamespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdInboxQueue)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.ReceivedEventNamespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("interval");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdSuspendingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.InboxInstance)
			.WithMany(p => p.InboxQueues)
			.HasForeignKey(d => d.IdInboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxQueue_IdInboxInstance");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.InboxQueues)
			.HasForeignKey(d => d.IdMessageType)
			.HasConstraintName("FK_InboxQueue_IdMessageType");

		entityBuilder.HasOne(d => d.ProcessingMode)
			.WithMany(p => p.InboxQueues)
			.HasForeignKey(d => d.IdProcessingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxQueue_IdProcessingMode");

		entityBuilder.HasOne(d => d.SuspendingMode)
			.WithMany(p => p.SuspendingModeInboxQueues)
			.HasForeignKey(d => d.IdSuspendingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxQueue_IdSuspendingMode");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxQueue>(ConfigureEntity);

		return modelBuilder;
	}
}
