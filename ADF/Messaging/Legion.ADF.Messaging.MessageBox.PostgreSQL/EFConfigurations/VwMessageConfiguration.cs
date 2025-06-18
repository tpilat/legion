using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwMessageConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwMessage>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwMessage> entityBuilder)
	{
		entityBuilder.ToView("VwMessage", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.MessageStatusCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.MessageStatusName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.MessageId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.BusinessId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.SessionId).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Publisher).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.MessageTypeCode)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.MessageTypeName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.MessageTypeNamespace)
			.IsRequired()
			.HasColumnType("varchar(1023)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwMessage>(ConfigureEntity);
}
