using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class VwInboxMessageProcessingLogConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxMessageProcessingLog>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxMessageProcessingLog> entityBuilder)
	{
		entityBuilder.ToView("VwInboxMessageProcessingLog", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxMessageProcessingLog).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdInboxMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.InboxMessageStatusCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.InboxMessageStatusName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxMessageProcessingLog>(ConfigureEntity);
}
