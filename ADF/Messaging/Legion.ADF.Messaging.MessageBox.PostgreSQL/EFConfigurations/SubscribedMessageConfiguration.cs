using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class SubscribedMessageConfiguration : IEntityTypeConfiguration<MessageBox.Model.SubscribedMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdSubscribedMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.SubscribedMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.SubscribedMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdSubscribedMessage);

		entityBuilder.ToTable("SubscribedMessage", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_SubscribedMessage_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdMessageProcessingStatus, "IXFK_SubscribedMessage_MessageProcessingStatus");

		entityBuilder.HasIndex(e => e.IdTopicSubscription, "IXFK_SubscribedMessage_TopicSubscription");

		entityBuilder.Property(e => e.IdSubscribedMessage)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uuid");

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
			.WithMany(p => p.SubscribedMessages)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_SubscribedMessage_MessageBoxInstance");

		entityBuilder.HasOne(d => d.MessageProcessingStatus)
			.WithMany(p => p.SubscribedMessages)
			.HasForeignKey(d => d.IdMessageProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_SubscribedMessage_IdMessageProcessingStatus");

		entityBuilder.HasOne(d => d.TopicSubscription)
			.WithMany(p => p.SubscribedMessages)
			.HasForeignKey(d => d.IdTopicSubscription)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_SubscribedMessage_IdTopicSubscription");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.SubscribedMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
