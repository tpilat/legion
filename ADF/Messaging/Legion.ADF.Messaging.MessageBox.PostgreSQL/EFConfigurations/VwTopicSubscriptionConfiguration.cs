using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwTopicSubscriptionConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwTopicSubscription>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwTopicSubscription> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwTopicSubscription> entityBuilder)
	{
		entityBuilder.ToView("VwTopicSubscription", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uuid");

		entityBuilder.Property(e => e.SubscriptionName)
			.IsRequired()
			.HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("interval");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.ProcessingModeCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.ProcessingModeName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdSuspendingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.SuspendingModeCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.SuspendingModeName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwTopicSubscription>(ConfigureEntity);
}
