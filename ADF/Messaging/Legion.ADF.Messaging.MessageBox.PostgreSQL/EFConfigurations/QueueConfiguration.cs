using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class QueueConfiguration : IEntityTypeConfiguration<MessageBox.Model.Queue>
{
	public const string PrimaryKeyFormatter = "{{\"IdQueue\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.Queue> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.Queue> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdQueue);

		entityBuilder.ToTable("Queue", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_Queue_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_Queue_MessageType");

		entityBuilder.HasIndex(e => e.IdProcessingMode, "IXFK_Queue_QueueProcessingMode");

		entityBuilder.HasIndex(e => e.IdSuspendingMode, "IXFK_Queue_QueueProcessingMode_02");

		entityBuilder.HasIndex(e => e.Name, "UQ_Queue_Name")
				.IsUnique();

		entityBuilder.HasIndex(e => e.ReceivedEventNamespace, "UQ_Queue_ReceivedEventNamespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdQueue)
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

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.Queues)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Queue_MessageBoxInstance");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.Queues)
			.HasForeignKey(d => d.IdMessageType)
			.HasConstraintName("FK_Queue_IdMessageType");

		entityBuilder.HasOne(d => d.ProcessingMode)
			.WithMany(p => p.Queues)
			.HasForeignKey(d => d.IdProcessingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Queue_IdProcessingMode");

		entityBuilder.HasOne(d => d.SuspendingMode)
			.WithMany(p => p.SuspendingModeQueues)
			.HasForeignKey(d => d.IdSuspendingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Queue_IdSuspendingMode");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.Queue>(ConfigureEntity);

		return modelBuilder;
	}
}
