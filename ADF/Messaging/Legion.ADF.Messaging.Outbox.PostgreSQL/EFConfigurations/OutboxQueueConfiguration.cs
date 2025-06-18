using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

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

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uuid");

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
