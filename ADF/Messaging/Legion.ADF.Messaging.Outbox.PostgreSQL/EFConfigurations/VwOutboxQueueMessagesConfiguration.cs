using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class VwOutboxQueueMessagesConfiguration : IEntityTypeConfiguration<Outbox.Model.VwOutboxQueueMessages>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwOutboxQueueMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwOutboxQueueMessages> entityBuilder)
	{
		entityBuilder.ToView("VwOutboxQueueMessages", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.OutboxQueueName)
			.IsRequired()
			.HasColumnType("varchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Outbox.Model.VwOutboxQueueMessages>(ConfigureEntity);
}
