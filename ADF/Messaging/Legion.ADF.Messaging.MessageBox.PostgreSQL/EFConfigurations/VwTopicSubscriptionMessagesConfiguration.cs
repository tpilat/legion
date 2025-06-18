using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwTopicSubscriptionMessagesConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwTopicSubscriptionMessages>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwTopicSubscriptionMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwTopicSubscriptionMessages> entityBuilder)
	{
		entityBuilder.ToView("VwTopicSubscriptionMessages", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uuid");

		entityBuilder.Property(e => e.SubscriptionName)
			.IsRequired()
			.HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.SubscriptionTimeoutForMessageProcessing).HasColumnType("interval");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uuid");

		entityBuilder.Property(e => e.TopicName)
			.IsRequired()
			.HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.TopicTimeoutForMessageProcessing).HasColumnType("interval");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwTopicSubscriptionMessages>(ConfigureEntity);
}
