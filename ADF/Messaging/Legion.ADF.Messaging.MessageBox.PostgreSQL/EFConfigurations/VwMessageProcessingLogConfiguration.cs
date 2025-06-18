using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwMessageProcessingLogConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwMessageProcessingLog>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwMessageProcessingLog> entityBuilder)
	{
		entityBuilder.ToView("VwMessageProcessingLog", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdMessageProcessingLog).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdQueuedMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdSubscribedMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.MessageProcessingStatusCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.MessageProcessingStatusName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwMessageProcessingLog>(ConfigureEntity);
}
