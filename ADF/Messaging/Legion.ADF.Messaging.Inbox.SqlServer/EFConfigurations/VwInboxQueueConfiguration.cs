using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class VwInboxQueueConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxQueue>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxQueue> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxQueue> entityBuilder)
	{
		entityBuilder.ToView("VwInboxQueue", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.ReceivedEventNamespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSequentialFIFO).HasColumnType("bit");

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("time(7)");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uniqueidentifier");

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

		entityBuilder.Property(e => e.MessageTypeCode).HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeName).HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeNamespace).HasColumnType("nvarchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxQueue>(ConfigureEntity);
}
