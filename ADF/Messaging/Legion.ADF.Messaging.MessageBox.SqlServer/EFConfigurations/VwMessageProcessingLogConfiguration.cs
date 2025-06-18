using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class VwMessageProcessingLogConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwMessageProcessingLog>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwMessageProcessingLog> entityBuilder)
	{
		entityBuilder.ToView("VwMessageProcessingLog", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdMessageProcessingLog).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdQueuedMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdSubscribedMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MessageProcessingStatusCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.MessageProcessingStatusName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwMessageProcessingLog>(ConfigureEntity);
}
