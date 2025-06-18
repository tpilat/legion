using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class QueuedMessageConfiguration : IEntityTypeConfiguration<MessageBox.Model.QueuedMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdQueuedMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.QueuedMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.QueuedMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdQueuedMessage);

		entityBuilder.ToTable("QueuedMessage", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_QueuedMessage_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdMessageProcessingStatus, "IXFK_QueuedMessage_MessageProcessingStatus");

		entityBuilder.HasIndex(e => e.IdQueue, "IXFK_QueuedMessage_Queue");

		entityBuilder.Property(e => e.IdQueuedMessage)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.AssignedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.QueuedMessages)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_QueuedMessage_MessageBoxInstance");

		entityBuilder.HasOne(d => d.MessageProcessingStatus)
			.WithMany(p => p.QueuedMessages)
			.HasForeignKey(d => d.IdMessageProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_QueuedMessage_IdMessageProcessingStatus");

		entityBuilder.HasOne(d => d.Queue)
			.WithMany(p => p.QueuedMessages)
			.HasForeignKey(d => d.IdQueue)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_QueuedMessage_IdQueue");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.QueuedMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
