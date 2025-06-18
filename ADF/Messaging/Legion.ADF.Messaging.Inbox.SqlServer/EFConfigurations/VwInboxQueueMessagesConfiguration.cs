using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class VwInboxQueueMessagesConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxQueueMessages>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxQueueMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxQueueMessages> entityBuilder)
	{
		entityBuilder.ToView("VwInboxQueueMessages", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.InboxQueueName)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSequentialFIFO).HasColumnType("bit");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxQueueMessages>(ConfigureEntity);
}
