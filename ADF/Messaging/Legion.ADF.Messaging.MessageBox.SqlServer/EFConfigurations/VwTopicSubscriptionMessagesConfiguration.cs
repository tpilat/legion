using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class VwTopicSubscriptionMessagesConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwTopicSubscriptionMessages>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwTopicSubscriptionMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwTopicSubscriptionMessages> entityBuilder)
	{
		entityBuilder.ToView("VwTopicSubscriptionMessages", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.SubscriptionName)
			.IsRequired()
			.HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.SubscriptionIsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.SubscriptionIsSequentialFIFO).HasColumnType("bit");

		entityBuilder.Property(e => e.SubscriptionTimeoutForMessageProcessing).HasColumnType("time(7)");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TopicName)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.TopisIsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.TopicIsSequentialFIFO).HasColumnType("bit");

		entityBuilder.Property(e => e.TopicTimeoutForMessageProcessing).HasColumnType("time(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwTopicSubscriptionMessages>(ConfigureEntity);
}
