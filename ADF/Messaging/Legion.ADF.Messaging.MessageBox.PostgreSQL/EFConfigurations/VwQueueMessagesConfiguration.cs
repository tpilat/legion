using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwQueueMessagesConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwQueueMessages>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwQueueMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwQueueMessages> entityBuilder)
	{
		entityBuilder.ToView("VwQueueMessages", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.QueueName)
			.IsRequired()
			.HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwQueueMessages>(ConfigureEntity);
}
