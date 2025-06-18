using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class VwMessageConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwMessage>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwMessage> entityBuilder)
	{
		entityBuilder.ToView("VwMessage", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MessageStatusCode)
			.IsRequired()
			.HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.MessageStatusName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.MessageId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.BusinessId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.SessionId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Publisher).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.MessageTypeCode)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeName)
			.IsRequired()
			.HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.MessageTypeNamespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwMessage>(ConfigureEntity);
}
