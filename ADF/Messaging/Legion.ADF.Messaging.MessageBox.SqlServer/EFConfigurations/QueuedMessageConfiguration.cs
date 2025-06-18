using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.AssignedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uniqueidentifier");

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
