using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class VwInboxQueueMessagesConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxQueueMessages>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxQueueMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxQueueMessages> entityBuilder)
	{
		entityBuilder.ToView("VwInboxQueueMessages", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.InboxQueueName)
			.IsRequired()
			.HasColumnType("varchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxQueueMessages>(ConfigureEntity);
}
