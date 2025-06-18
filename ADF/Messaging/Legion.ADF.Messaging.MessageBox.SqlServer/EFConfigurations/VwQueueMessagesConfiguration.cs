using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class VwQueueMessagesConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwQueueMessages>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwQueueMessages> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwQueueMessages> entityBuilder)
	{
		entityBuilder.ToView("VwQueueMessages", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.QueueName)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSequentialFIFO).HasColumnType("bit");

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwQueueMessages>(ConfigureEntity);
}
