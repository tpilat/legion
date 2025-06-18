using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class VwInboxMessageProcessingLogConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxMessageProcessingLog>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxMessageProcessingLog> entityBuilder)
	{
		entityBuilder.ToView("VwInboxMessageProcessingLog", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxMessageProcessingLog).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdInboxMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdInboxMessageStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.InboxMessageStatusCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.InboxMessageStatusName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxMessageProcessingLog>(ConfigureEntity);
}
