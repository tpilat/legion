using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class VwOutboxQueueMessagesConfiguration : IEntityTypeConfiguration<Outbox.Model.VwOutboxQueueMessages>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwOutboxQueueMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwOutboxQueueMessages> entityBuilder)
	{
		entityBuilder.ToView("VwOutboxQueueMessages", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.OutboxQueueName)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSequentialFIFO).HasColumnType("bit");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Outbox.Model.VwOutboxQueueMessages>(ConfigureEntity);
}
