using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class TopicSubscriptionConfiguration : IEntityTypeConfiguration<MessageBox.Model.TopicSubscription>
{
	public const string PrimaryKeyFormatter = "{{\"IdTopicSubscription\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.TopicSubscription> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.TopicSubscription> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdTopicSubscription);

		entityBuilder.ToTable("TopicSubscription", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_TopicSubscription_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdProcessingMode, "IXFK_TopicSubscription_QueueProcessingMode");

		entityBuilder.HasIndex(e => e.IdSuspendingMode, "IXFK_TopicSubscription_QueueProcessingMode_02");

		entityBuilder.HasIndex(e => e.IdTopic, "IXFK_TopicSubscription_Topic");

		entityBuilder.HasIndex(e => new { e.IdTopic, e.SubscriptionName }, "UQ_TopicSubscription_IdTopic_SubscriptionName")
				.IsUnique();

		entityBuilder.HasIndex(e => e.ReceivedEventNamespace, "UQ_TopicSubscription_ReceivedEventNamespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdTopicSubscription)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uuid");

		entityBuilder.Property(e => e.SubscriptionName)
			.IsRequired()
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.ReceivedEventNamespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("interval");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdSuspendingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.TopicSubscriptions)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_TopicSubscription_MessageBoxInstance");

		entityBuilder.HasOne(d => d.ProcessingMode)
			.WithMany(p => p.TopicSubscriptions)
			.HasForeignKey(d => d.IdProcessingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_TopicSubscription_IdProcessingMode");

		entityBuilder.HasOne(d => d.SuspendingMode)
			.WithMany(p => p.SuspendingModeTopicSubscriptions)
			.HasForeignKey(d => d.IdSuspendingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_TopicSubscription_IdSuspendingMode");

		entityBuilder.HasOne(d => d.Topic)
			.WithMany(p => p.TopicSubscriptions)
			.HasForeignKey(d => d.IdTopic)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_TopicSubscription_IdTopic");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.TopicSubscription>(ConfigureEntity);

		return modelBuilder;
	}
}
