using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class QueuedMessageConfiguration : IEntityTypeConfiguration<MBox.Model.QueuedMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdQueuedMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.QueuedMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.QueuedMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdQueuedMessage);

		entityBuilder.ToTable("QueuedMessage", "mbox");

		entityBuilder.HasIndex(e => e.IdMessage, "IXFK_QueuedMessage_IdMessage");

		entityBuilder.HasIndex(e => e.IdMessageProcessingStatus, "IXFK_QueuedMessage_IdMessageProcessingStatus");

		entityBuilder.HasIndex(e => e.IdQueue, "IXFK_QueuedMessage_IdQueue");

		entityBuilder.Property(e => e.IdQueuedMessage).ValueGeneratedNever();

		entityBuilder.Property(e => e.QueuedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.LastProcessedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.TerminatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.HasOne(d => d.Message)
			.WithMany(p => p.QueuedMessages)
			.HasForeignKey(d => d.IdMessage)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_QueuedMessage_IdMessage");

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
		modelBuilder.Entity<MBox.Model.QueuedMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
