using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class OutboxQueueConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxQueue>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxQueue\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxQueue> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxQueue> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxQueue);

		entityBuilder.ToTable("OutboxQueue", "outbox");

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_OutboxQueue_MessageType");

		entityBuilder.HasIndex(e => e.IdOutboxInstance, "IXFK_OutboxQueue_OutboxInstance");

		entityBuilder.HasIndex(e => e.IdProcessingMode, "IXFK_OutboxQueue_OutboxQueueProcessingMode");

		entityBuilder.HasIndex(e => e.IdSuspendingMode, "IXFK_OutboxQueue_OutboxQueueProcessingMode_02");

		entityBuilder.HasIndex(e => e.Name, "UQ_OutboxQueue_Name")
				.IsUnique();

		entityBuilder.HasIndex(e => e.ReceivedEventNamespace, "UQ_OutboxQueue_ReceivedEventNamespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdOutboxQueue)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.ReceivedEventNamespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSequentialFIFO).HasColumnType("bit");

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("time(7)");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdSuspendingMode).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.OutboxQueues)
			.HasForeignKey(d => d.IdMessageType)
			.HasConstraintName("FK_OutboxQueue_IdMessageType");

		entityBuilder.HasOne(d => d.OutboxInstance)
			.WithMany(p => p.OutboxQueues)
			.HasForeignKey(d => d.IdOutboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxQueue_IdOutboxInstance");

		entityBuilder.HasOne(d => d.ProcessingMode)
			.WithMany(p => p.OutboxQueues)
			.HasForeignKey(d => d.IdProcessingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxQueue_IdProcessingMode");

		entityBuilder.HasOne(d => d.SuspendingMode)
			.WithMany(p => p.SuspendingModeOutboxQueues)
			.HasForeignKey(d => d.IdSuspendingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxQueue_IdSuspendingMode");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxQueue>(ConfigureEntity);

		return modelBuilder;
	}
}
