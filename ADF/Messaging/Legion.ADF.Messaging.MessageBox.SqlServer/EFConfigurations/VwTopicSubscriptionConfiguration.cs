using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class VwTopicSubscriptionConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwTopicSubscription>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwTopicSubscription> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwTopicSubscription> entityBuilder)
	{
		entityBuilder.ToView("VwTopicSubscription", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.SubscriptionName)
			.IsRequired()
			.HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.IsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSequentialFIFO).HasColumnType("bit");

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("time(7)");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ProcessingModeCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.ProcessingModeName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.IdSuspendingMode).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.SuspendingModeCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.SuspendingModeName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwTopicSubscription>(ConfigureEntity);
}
