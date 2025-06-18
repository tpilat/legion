using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class VwOutboxQueueConfiguration : IEntityTypeConfiguration<Outbox.Model.VwOutboxQueue>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwOutboxQueue> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwOutboxQueue> entityBuilder)
	{
		entityBuilder.ToView("VwOutboxQueue", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uniqueidentifier");

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

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");

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
		=> modelBuilder.Entity<Outbox.Model.VwOutboxQueue>(ConfigureEntity);
}
