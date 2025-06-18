using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uniqueidentifier");

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
